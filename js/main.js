document.addEventListener("DOMContentLoaded", () => {
  const grid = document.querySelector(".projects-grid");

  if (!grid) return;

  const projectCount = grid.children.length;

  if (projectCount % 2 === 1) {
    grid.classList.add("odd-count");
  }
});