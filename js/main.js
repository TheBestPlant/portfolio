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
  const fileButtonsContainer = document.getElementById("file-buttons");

  if (!codeDisplay || !fileButtonsContainer) return;

  const fileMap = {
    audio: [
      {
        name: "Room Sounds",
        path: "../../assets/code/papertrailcode/audio/RoomSound.cs"
      }
    ],

    dialogue: [
      {
        name: "Dialogue Display",
        path: "../../assets/code/papertrailcode/dialogue/DialogueDisplay.cs"
      },
      {
        name: "Dialogue System",
        path: "../../assets/code/papertrailcode/dialogue/DialogueSystem.cs"
      }
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
      { name: "Note", path: "../../assets/code/papertrailcode/interactables/Note.cs" },
      { name: "Note UI", path: "../../assets/code/papertrailcode/interactables/NoteUIManager.cs" },
      { name: "Refill", path: "../../assets/code/papertrailcode/interactables/WaterRefilStation.cs" }
    ],

    loadlevel: [
      { name: "Load Level", path: "../../assets/code/papertrailcode/loadlevel/LoadLevelButtonBridge.cs" }
    ],

    monsters: [
      { name: "Jumpscare", path: "../../assets/code/papertrailcode/monsters/JumpscareAction.cs" },
      { name: "Monster Patrol", path: "../../assets/code/papertrailcode/monsters/MonsterPatrolChase.cs" }
    ],

    projectiles: [
      { name: "Flame", path: "../../assets/code/papertrailcode/projectiles/Flame.cs" },
      { name: "Water", path: "../../assets/code/papertrailcode/projectiles/WaterProjectile.cs" },
      { name: "Water Thrower", path: "../../assets/code/papertrailcode/projectiles/WaterThrower.cs" }
    ],

    shadowfollow: [
      { name: "Shadow Follow", path: "../../assets/code/papertrailcode/shadowfollow/ShadowFollow.cs" }
    ],

    uitoggle: [
      { name: "UI Toggle", path: "../../assets/code/papertrailcode/uitoggle/UIToggleManager.cs" }
    ]
  };

  function loadCodeFile(fileName) {
    fetch(fileName)
      .then(response => {
        if (!response.ok) {
          throw new Error(`Could not load ${fileName}`);
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

  document.querySelectorAll("[data-folder]").forEach(folderButton => {
    folderButton.addEventListener("click", () => {
      document.querySelectorAll("[data-folder]").forEach(btn =>
        btn.classList.remove("active")
      );

      folderButton.classList.add("active");

      const folder = folderButton.dataset.folder;
      const files = fileMap[folder];

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
