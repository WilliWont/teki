// When the user scrolls the page, execute myFunction
//window.onscroll = function () { myFunction() };
//https://www.w3schools.com/howto/howto_js_navbar_sticky.asp
// Get the navbar
var navbar = document.getElementById("site-header");
var $navbar = $("#site-header");
// Get the offset position of the navbar
//var sticky = navbar.offsetTop;

//// Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
//function myFunction() {
//    if (window.pageYOffset >= sticky) {
//        $navbar.addClass('dark-nav');
//    } else {
//        $navbar.removeClass('dark-nav');
//    }
//}

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