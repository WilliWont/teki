// Get the navbar
var navbar = document.getElementById("site-header");
var $navbar = $("#site-header");

// by nimaek
//https://stackoverflow.com/a/34332847/11620610
jQuery(window).on('scroll', function () {
    var top = jQuery(window).scrollTop(),
        divBottom = jQuery('.article-header').offset().top + jQuery('.article-header').outerHeight();
    if (divBottom*6/9 > top) {
        $navbar.removeClass('dark-nav');
    } else {
        $navbar.addClass('dark-nav');
    }
});