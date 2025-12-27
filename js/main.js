document.addEventListener("DOMContentLoaded", () => {
  const grid = document.querySelector(".projects-grid");

  if (grid) {
    const projectCount = grid.children.length;

    if (projectCount % 2 === 1) {
      grid.classList.add("odd-count");
    }
  }
});

document.addEventListener("DOMContentLoaded", () => {
  const codeDisplay = document.getElementById("code-display");
  if (!codeDisplay) return;

  function loadCodeFile(path) {
    fetch(path)
      .then(response => {
        if (!response.ok) {
          throw new Error(`Could not load ${path}`);
        }
        return response.text();
      })
      .then(text => {
        codeDisplay.textContent = text;
        Prism.highlightElement(codeDisplay);
      })
      .catch(error => {
        codeDisplay.textContent = error.message;
      });
  }

  const fileButtons = document.querySelectorAll("[data-file]");
  if (fileButtons.length > 0) {
    fileButtons.forEach(btn => {
      btn.addEventListener("click", () => {
        fileButtons.forEach(b => b.classList.remove("active"));
        btn.classList.add("active");
        loadCodeFile(btn.dataset.file);
      });
    });

    fileButtons[0].classList.add("active");
    loadCodeFile(fileButtons[0].dataset.file);
    return;
  }

  const fileButtonsContainer = document.getElementById("file-buttons");
  if (!fileButtonsContainer) return;

  const fileMap = {
      audio: [
        { name: "Room Sounds", path: "../../assets/code/papertrailcode/audio/RoomSound.cs" }
      ],

      dialogue: [
        { name: "Dialogue Display", path: "../../assets/code/papertrailcode/dialogue/DialogueDisplay.cs" },
        { name: "Dialogue System", path: "../../assets/code/papertrailcode/dialogue/DialogueSystem.cs" }
      ],

      door: [
        { name: "Door Code", path: "../../assets/code/papertrailcode/door/Door.cs" },
        { name: "Door Interactor", path: "../../assets/code/papertrailcode/door/DoorInteractor.cs" },
        { name: "Keycard", path: "../../assets/code/papertrailcode/door/Keycard.cs" },
        { name: "Keypad", path: "../../assets/code/papertrailcode/door/Keypad.cs" },
        { name: "Keypad Interactor", path: "../../assets/code/papertrailcode/door/KeypadInteractor.cs" },
        { name: "Keypad UI", path: "../../assets/code/papertrailcode/door/KeypadUIManager.cs" },
        { name: "Riddle UI", path: "../../assets/code/papertrailcode/door/RiddleUIManager.cs" }
      ],

      interactables: [
        { name: "Grab Objects", path: "../../assets/code/papertrailcode/interactables/GrabObjects.cs" },
        { name: "Note Code", path: "../../assets/code/papertrailcode/interactables/Note.cs" },
        { name: "Note UI", path: "../../assets/code/papertrailcode/interactables/NoteUIManager.cs" },
        { name: "Refill Code", path: "../../assets/code/papertrailcode/interactables/WaterRefilStation.cs" }
      ],

      loadlevel: [
        { name: "Load Level", path: "../../assets/code/papertrailcode/loadlevel/LoadLevelButtonBridge.cs" }
      ],

      monsters: [
        { name: "Jumpscare Code", path: "../../assets/code/papertrailcode/monsters/JumpscareAction.cs" },
        { name: "Monster Patrol", path: "../../assets/code/papertrailcode/monsters/MonsterPatrolChase.cs" }
      ],

      projectiles: [
        { name: "Flame Code", path: "../../assets/code/papertrailcode/projectiles/Flame.cs" },
        { name: "Water Code", path: "../../assets/code/papertrailcode/projectiles/WaterProjectile.cs" },
        { name: "Water Thrower", path: "../../assets/code/papertrailcode/projectiles/WaterThrower.cs" }
      ],

      shadowfollow: [
        { name: "Shadow Follow", path: "../../assets/code/papertrailcode/shadowfollow/ShadowFollow.cs" }
      ],

      uitoggle: [
        { name: "UI Toggle", path: "../../assets/code/papertrailcode/uitoggle/UIToggleManager.cs" }
      ],

      audioscripts: [
        { name: "Audio Manager", path: "../../assets/code/whiskerwoodscode/audio/AudioManager.cs" }
      ],

      characterselect: [
        { name: "Character", path: "../../assets/code/whiskerwoodscode/characterselect/Character.cs" },
        { name: "Character Database", path: "../../assets/code/whiskerwoodscode/characterselect/CharacterDatabase.cs" },
        { name: "Character Manager", path: "../../assets/code/whiskerwoodscode/characterselect/CharacterManager.cs" },
        { name: "Player Code", path: "../../assets/code/whiskerwoodscode/characterselect/Player.cs" }
      ],

      hazard: [
        { name: "Dialogue Balloon Edit", path: "../../assets/code/whiskerwoodscode/hazard/GMGDialogueBalloonAction.cs" },
        { name: "Hazard Destruction", path: "../../assets/code/whiskerwoodscode/hazard/HazardDestruction.cs" }
      ],

      inventory: [
        { name: "Dropped Item", path: "../../assets/code/whiskerwoodscode/inventory/DroppedItem.cs" },
        { name: "Inventory Item", path: "../../assets/code/whiskerwoodscode/inventory/InventoryItem.cs" },
        { name: "Inventory Item Manager", path: "../../assets/code/whiskerwoodscode/inventory/InventoryItemsManager.cs" },
        { name: "Inventory Slot", path: "../../assets/code/whiskerwoodscode/inventory/InventorySlot.cs" },
        { name: "Item Code", path: "../../assets/code/whiskerwoodscode/inventory/Item.cs" },
        { name: "Item Database", path: "../../assets/code/whiskerwoodscode/inventory/ItemDatabase.cs" },
        { name: "Item Database Manager", path: "../../assets/code/whiskerwoodscode/inventory/ItemDatabaseManager.cs" },
        { name: "UI Manager", path: "../../assets/code/whiskerwoodscode/inventory/UIManager.cs" }
      ],

      planting: [
        { name: "Day Manager", path: "../../assets/code/whiskerwoodscode/planting/DayManager.cs" },
        { name: "Day Pass Button", path: "../../assets/code/whiskerwoodscode/planting/DayPassButton.cs" },
        { name: "Plant Code", path: "../../assets/code/whiskerwoodscode/planting/Plant.cs" },
        { name: "Plant Manager", path: "../../assets/code/whiskerwoodscode/planting/PlantManager.cs" },
        { name: "Seed Plant Manager", path: "../../assets/code/whiskerwoodscode/planting/SeedPlantManager.cs" }
      ],

      quest: [
        { name: "Game Data", path: "../../assets/code/whiskerwoodscode/quest/GameData.cs" },
        { name: "Quest Code", path: "../../assets/code/whiskerwoodscode/quest/Quest.cs" },
        { name: "Quest List Manager", path: "../../assets/code/whiskerwoodscode/quest/QuestListManager.cs" }
      ],

      selling: [
        { name: "Customer Code", path: "../../assets/code/whiskerwoodscode/selling/Customer.cs" },
        { name: "Customer Manager", path: "../../assets/code/whiskerwoodscode/selling/CustomerManager.cs" }
      ],

      spawn: [
        { name: "Level Loader", path: "../../assets/code/whiskerwoodscode/spawn/LevelLoader.cs" },
        { name: "Quit Game", path: "../../assets/code/whiskerwoodscode/spawn/QuitGameScript.cs" },
        { name: "Spawn Manager Forest", path: "../../assets/code/whiskerwoodscode/spawn/SceneSpawnManagerForest.cs" },
        { name: "Spawn Manager Greenhouse", path: "../../assets/code/whiskerwoodscode/spawn/SceneSpawnManagerGreenhouse.cs" },
        { name: "Spawn Manager Outside", path: "../../assets/code/whiskerwoodscode/spawn/SceneSpawnManagerOutside.cs" },
        { name: "Spawn Manager Shop", path: "../../assets/code/whiskerwoodscode/spawn/SceneSpawnManagerShop.cs" },
        { name: "Spawn Manager Shop Window", path: "../../assets/code/whiskerwoodscode/spawn/SceneSpawnManagerShopWindow.cs" },
        { name: "Spawn Manager", path: "../../assets/code/whiskerwoodscode/spawn/SpawnManager.cs" }
      ]
  };

  document.querySelectorAll("[data-folder]").forEach(folderButton => {
    folderButton.addEventListener("click", () => {
      document.querySelectorAll("[data-folder]").forEach(b =>
        b.classList.remove("active")
      );

      folderButton.classList.add("active");
      const files = fileMap[folderButton.dataset.folder];
      fileButtonsContainer.innerHTML = "";

      files.forEach(file => {
        const btn = document.createElement("button");
        btn.className = "top-button";
        btn.textContent = file.name;

        btn.addEventListener("click", () => {
          document.querySelectorAll("#file-buttons .top-button").forEach(b =>
            b.classList.remove("active")
          );

          btn.classList.add("active");
          loadCodeFile(file.path);
        });

        fileButtonsContainer.appendChild(btn);
      });

      if (files.length > 0) {
        fileButtonsContainer.firstChild.classList.add("active");
        loadCodeFile(files[0].path);
      }
    });
  });

  const firstFolderButton = document.querySelector("[data-folder]");
  if (firstFolderButton) {
    firstFolderButton.click();
  }
});
