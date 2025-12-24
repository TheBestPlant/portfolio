using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using GameNetcodeStuff;
using UnityEngine.Networking;
using System.Linq;
using System.Collections.Generic;

namespace _760Modtest.Patches
{
    public class LootbugRole : MonoBehaviour
    {
        //0 for roamer, 1 for attacker, 2 for guarder
        public int role = 0;
    }

    [BepInPlugin(modGUID, modName, modVersion)]
    public class LethalLootBugs : BaseUnityPlugin
    {
        private const string modGUID = "SheTheys.LethalLootBugs";
        private const string modName = "LethalLootBugs";
        private const string modVersion = "1.0.2";

        private readonly Harmony harmony = new Harmony(modGUID);
        public static ManualLogSource mls;
        public static AudioClip LootBugAudioClip;
        private static System.Random rng = new System.Random();

        public static HoarderBugAI currentGuardBug = null;
        public static Vector3 nestPosition = Vector3.zero;
        public static bool nestPositionInitialized = false;


        //Dunno if these can be accessible? but add the bugs to them anyway
        public List<HoarderBugAI> attackingBugs = new List<HoarderBugAI>();
        private List<HoarderBugAI> roamingBugs = new List<HoarderBugAI>();
        private List<HoarderBugAI> guardingBugs = new List<HoarderBugAI>();

        private void Awake()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource("LethalLootBugs");
            mls.LogInfo("Loaded LethalLootBugs and applying patches.");

            // Apply all Harmony patches
            harmony.PatchAll(typeof(LethalLootBugs));

        }

        // Patch to make only Hoarder Bugs spawn
        // PLUS INFESTATION
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.LoadNewLevel))]
        [HarmonyPrefix]
        static bool Only_Bugs_Spawn(ref SelectableLevel newLevel)
        {
            float spawnMultiplier = 4f; // we can play w this
            foreach (var item in newLevel.Enemies)
            {
                EnemyType enemy = item.enemyType;
                if (enemy.enemyPrefab.GetComponent<HoarderBugAI>() != null &&
                    enemy.enemyPrefab.name.Contains("HoarderBug"))
                {
                    item.rarity = 999; // boost Hoarder Bug spawn

                    // inc spawn rate
                    enemy.MaxCount = Mathf.CeilToInt(enemy.MaxCount * spawnMultiplier);
                    // group size
                    enemy.spawnInGroupsOf = Mathf.CeilToInt(enemy.spawnInGroupsOf * spawnMultiplier);
                    // powerlevel = spawn cost, lower to allow infestation >:)
                    enemy.PowerLevel = Mathf.Max(0.1f, enemy.PowerLevel / spawnMultiplier);
                    LethalLootBugs.mls.LogInfo("Infestation!! MaxCount={enemy.MaxCount}, GroupSize={enemy.spawnInGroupsOf}, PowerLevel={enemy.PowerLevel}");
                }
                else
                {
                    item.rarity = 0; // disable others
                }
            }

            foreach (var item in newLevel.OutsideEnemies)
            {
                item.rarity = 0; // disable outside enemies
            }

            return true;
        }

        // split into roamers and attackers
        [HarmonyPatch(typeof(HoarderBugAI), "Start")]
        [HarmonyPostfix]
        static void DivideBehavior(HoarderBugAI __instance)
        {
            if (__instance == null)
                return;

            bool isAggressive = rng.NextDouble() < 0.25; //half and half
            PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
            __instance.gameObject.AddComponent<LootbugRole>();

            if (isAggressive && players.Length > 0)
            {
                // target closest player
                PlayerControllerB target = players
                    .OrderBy(p => Vector3.Distance(__instance.transform.position, p.transform.position))
                    .FirstOrDefault();

                // reflect to get private fields safely
                var t = typeof(HoarderBugAI);
                var isAngryField = t.GetField("isAngry", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                var inChaseField = t.GetField("inChase", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                var angryAtPlayerField = t.GetField("angryAtPlayer", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                if (isAngryField != null)
                    isAngryField.SetValue(__instance, true);
                if (inChaseField != null)
                    inChaseField.SetValue(__instance, true);
                if (angryAtPlayerField != null)
                    angryAtPlayerField.SetValue(__instance, target);

                mls.LogInfo($"[LethalLootBugs] aggro bug targeting {target.playerUsername}");

                //Updating component to have the attacker role
                LootbugRole lootbugRole = __instance.GetComponent<LootbugRole>();
                lootbugRole.role = 1;


                //TODO: may want to add to manager lists?
            }

            //Of the non aggressive bugs, split them into roamer and guard
            else
            {
                bool isGuard = rng.NextDouble() < 0.5;
                if (isGuard) {
                    mls.LogInfo("[LethalLootBugs] Guard spawned");
                    LootbugRole lootbugRole = __instance.GetComponent<LootbugRole>();
                    lootbugRole.role = 2;
                }

                else
                {
                    mls.LogInfo("[LethalLootBugs] roaming bugs spawned");
                }

                //Roaming role value is 0 and component is already added, don't need to update anything

                //TODO: may want to add to manager lists?
            }
        }

        // Infinite sprint patch
        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void InfiniteSprintPatch(ref float ___sprintMeter)
        {
            ___sprintMeter = 1f;
        }


        //All changes are done after the original code runs

        [HarmonyPatch(typeof(HoarderBugAI), "DoAIInterval")]
        [HarmonyPostfix]
        //instance refers to the instance of the ai
        static void NewLootbugAI(HoarderBugAI __instance)
        {
            //access behavior index
            int behaviorState = __instance.currentBehaviourStateIndex;

            //// assign nearest bug to nest if there's not a guarder
            //if (LethalLootBugs.currentGuardBug == null)
            //{
            //    HoarderBugAI[] allBugs = GameObject.FindObjectsOfType<HoarderBugAI>();
            //    if (allBugs.Length == 0) { return; }

            //    HoarderBugAI closest = allBugs
            //        .Where(b => b != null && !b.isEnemyDead)
            //        .OrderBy(b => Vector3.Distance(b.transform.position, LethalLootBugs.nestPosition))
            //        .FirstOrDefault();

            //    if (closest != null)
            //    {
            //        LootbugRole newRole = closest.GetComponent<LootbugRole>();
            //        if (newRole != null)
            //        {
            //            newRole.role = 2;
            //            LethalLootBugs.currentGuardBug = closest;

            //            LethalLootBugs.mls.LogInfo("new guard bug");
            //        }
            //    }
            //}

            //Access the component with the role int 
            LootbugRole lootbugRole = __instance.gameObject.GetComponent<LootbugRole>();

            if (lootbugRole == null) { return; }

            switch (lootbugRole.role)
            {
                //Roamers
                case 0:

                    
                    //state 0 is the index for the search behavior
                    if (behaviorState == 0)
                    {
                        //Making a new list of the items so they can be sorted without exploding anything else
                        List<GrabbableObject> items = new List<GrabbableObject>();

                        //Loops through and moves over all the valid items
                        //I don't *think* they should be null but just in case
                        for (int i = 0; i < HoarderBugAI.grabbableObjectsInMap.Count; i++)
                        {
                            GameObject thing = HoarderBugAI.grabbableObjectsInMap[i];
                            if (thing != null)
                            {
                                GrabbableObject thingGrabbable = thing.GetComponent<GrabbableObject>();
                                if (thingGrabbable != null)
                                {
                                    items.Add(thingGrabbable);
                                }
                            }
                        }

                        //Goes through comparing two items to each other down the whole list
                        //Sorts them greatest to least
                        items.Sort((item1, item2) => item2.scrapValue.CompareTo(item1.scrapValue));

                        //If there's less than 3 items on the list just go for the top one
                        if (items.Count < 3 && items.Count > 0)
                        {
                            __instance.targetItem = items[0];
                        }
                        //Randomly pick one from the top 3
                        else if (items.Count > 0)
                        {
                            int selection = UnityEngine.Random.Range(0, 3);
                            __instance.targetItem = items[selection];
                        }
                        //Do nothing if there's no items
                        else
                        {
                            return;
                        }
                    }

                    //Keep it in state 0
                    else { __instance.currentBehaviourStateIndex = 0; }

                break;

                //Attackers
                case 1:
                    if (__instance == null || __instance.isEnemyDead)
                        return;

                    PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
                    if (player == null)
                        return;

                    var type = typeof(HoarderBugAI);
                    var isAngryField = type.GetField("isAngry", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    var angryAtPlayerField = type.GetField("angryAtPlayer", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                    //if it sees the player, attack >:(
                    if (__instance.watchingPlayerNearPosition)
                    {
                        isAngryField.SetValue(__instance, true);

                        Vector3 dir = player.transform.position - __instance.transform.position;
                        if (dir.sqrMagnitude > 0.01f)
                            __instance.transform.rotation = Quaternion.LookRotation(dir);

                        __instance.angryTimer = 60f;
                        __instance.currentBehaviourStateIndex = 3;
                        __instance.agent?.SetDestination(player.transform.position);

                        Debug.Log($"[LethalLootBugs] {__instance.name} is now angry and chasing {player.playerUsername}");
                    }
                    break;

                //Guarders
                case 2:

                    //// if current dies, set guard to null
                    //if (LethalLootBugs.currentGuardBug != null && LethalLootBugs.currentGuardBug.isEnemyDead)
                    //{
                    //    LethalLootBugs.mls.LogInfo("guard bug dead! get new guy");
                    //    LethalLootBugs.currentGuardBug = null;
                    //}

                    //Bug stays near nest
                    float dist = Vector3.Distance(__instance.transform.position, LethalLootBugs.nestPosition);
                    if (dist > 3f)
                    {
                        __instance.agent.SetDestination(LethalLootBugs.nestPosition);
                    }

                    break;

            }
        }
    }
}

