document.getElementById('themeSwitcher').addEventListener('click', function () {
    var currentTheme = document.body.classList.contains('dark') ? 'dark' : 'light';
    var newTheme = currentTheme === 'light' ? 'dark' : 'light';
    document.body.classList.toggle('dark');
    document.body.classList.toggle('light');
    document.cookie = "theme=" + newTheme + "; path=/";
});