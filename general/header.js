(function () {
	const headerScript = document.querySelector('script[src*="general/header.js"]');
	const scriptSrc = headerScript ? headerScript.getAttribute("src") : "general/header.js";
	const rootPrefix = scriptSrc.replace(/general\/header\.js.*$/, "");

	const headerHTML = `
		<header class="site-header">
			<a href="${rootPrefix || "/"}" class="site-title">
				Savannah Balistreri <br> Gameplay Programmer and Technical Producer
			</a>
			<nav class="nav-links">
				<a href="${rootPrefix || "/"}">Home</a>
				<a href="${rootPrefix}projects">Projects</a>
				<a href="${rootPrefix}about">About Me</a>
			</nav>
		</header>
	`;

	function renderHeader() {
		const placeholder = document.getElementById("header-placeholder");
		if (placeholder) {
			placeholder.innerHTML = headerHTML;
		}
	}

	if (document.readyState === "loading") {
		document.addEventListener("DOMContentLoaded", renderHeader);
	} else {
		renderHeader();
	}
})();