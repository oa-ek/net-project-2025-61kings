const themeToggleBtn = document.getElementById("themeToggleBtn");
const themeIcon = document.getElementById("themeIcon");

if (localStorage.getItem("theme") === "dark") {
    document.body.classList.add("dark-theme");
    themeIcon.src = "images/theme-icon.png"; 
} else {
    document.body.classList.remove("dark-theme");
    themeIcon.src = "images/theme-iconBlack.png"; 
}

themeToggleBtn.addEventListener("click", function () {
    if (document.body.classList.contains("dark-theme")) {
        document.body.classList.remove("dark-theme");
        themeIcon.src = "images/theme-iconBlack.png"; 
        localStorage.setItem("theme", "light"); 
    } else {
        document.body.classList.add("dark-theme");
        themeIcon.src = "images/theme-icon.png"; 
        localStorage.setItem("theme", "dark"); 
    }
});
