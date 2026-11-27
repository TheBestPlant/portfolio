(function () {
	const contactScript = document.querySelector('script[src*="general/contact.js"]');
	const scriptSrc = contactScript ? contactScript.getAttribute("src") : "general/contact.js";
	const rootPrefix = scriptSrc.replace(/general\/contact\.js.*$/, "");

	const contactHTML = `
		<div class="contact-bar">
			<a href="https://www.linkedin.com/in/savannah-balistreri-3350a6239/" target="_blank" rel="noopener noreferrer" class="contact-button">LinkedIn</a>
			<a href="mailto:savannahbsf@gmail.com" class="contact-button">Savannahbsf@gmail.com</a>
			<a href="tel:+14155954633" class="contact-button">(415) 595-4633</a>
			<a href="${rootPrefix}assets/resume/resume.pdf" target="_blank" rel="noopener noreferrer" class="contact-button">Resume</a>
		</div>
	`;

	function renderContact() {
		const placeholder = document.getElementById("contact-placeholder");
		if (placeholder) {
			placeholder.innerHTML = contactHTML;
		} else {
			document.body.insertAdjacentHTML("beforeend", contactHTML);
		}
	}

	if (document.readyState === "loading") {
		document.addEventListener("DOMContentLoaded", renderContact);
	} else {
		renderContact();
	}
})();