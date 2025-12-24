document.addEventListener("DOMContentLoaded", () => {
  const grid = document.querySelector(".projects-grid");

  if (!grid) return;

  const projectCount = grid.children.length;

  if (projectCount % 2 === 1) {
    grid.classList.add("odd-count");
  }
});

document.addEventListener("DOMContentLoaded", () => {
  const codeDisplay = document.getElementById("code-display");
  const codeNavButtons = document.querySelectorAll(".code-nav button");

  function loadCodeFile(fileName) {
    fetch(`code/${fileName}`)
      .then(response => {
        if (!response.ok) {
          throw new Error(`Could not load ${fileName}`);
        }
        return response.text();
      })
      .then(text => {
        codeDisplay.textContent = text;
      })
      .catch(error => {
        codeDisplay.textContent = error.message;
      });
  }

  if (codeNavButtons.length > 0) {
    loadCodeFile(codeNavButtons[0].dataset.file);
  }

  codeNavButtons.forEach(button => {
    button.addEventListener("click", () => {
      loadCodeFile(button.dataset.file);
    });
  });
});
