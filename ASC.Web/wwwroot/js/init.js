// Automobile Service Center - Init JS

document.addEventListener('DOMContentLoaded', function () {

    // Initialize Materialize components
    M.AutoInit();

    // Collapsible sidebar navigation
    var collapsibles = document.querySelectorAll('.collapsible');
    M.Collapsible.init(collapsibles);

    // Tooltips
    var tooltips = document.querySelectorAll('.tooltipped');
    M.Tooltip.init(tooltips);

    // Modals
    var modals = document.querySelectorAll('.modal');
    M.Modal.init(modals);

    // Auto-dismiss alerts after 4 seconds
    var alerts = document.querySelectorAll('.auto-dismiss');
    alerts.forEach(function (el) {
        setTimeout(function () {
            el.style.transition = 'opacity 0.5s';
            el.style.opacity = '0';
            setTimeout(function () { el.remove(); }, 500);
        }, 4000);
    });

});

// Prevent back/forward button caching on secure pages (Lab 4)
window.onpageshow = function (event) {
    if (event.persisted) {
        window.location.reload();
    }
};

// Block right-click (Lab 4 requirement)
document.addEventListener('contextmenu', function(e) {
    e.preventDefault();
});
