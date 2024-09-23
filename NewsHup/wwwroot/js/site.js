// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(".owl-carousel").owlCarousel();
});
var owl = $('.owl-carousel');
owl.owlCarousel({
    loop: true,
    mouseDrag: true,
    touchDrag: true,
    autoplay: true,
    autoplayHoverPause: true,
    autoplayTimeout: 1500,
    pullDrag: false,
    dots: false,
    navSpeed: 700,
    navText: ['', ''],
    items: 1,
    nav: true,
    rtl: true,
});
$('.play').on('click', function () {
    owl.trigger('play.owl.autoplay', [1000])
})
$('.stop').on('click', function () {
    owl.trigger('stop.owl.autoplay')
})
$(document).ready(function () {

    const navbarLinks = $('.navbar .main-ul a');

    navbarLinks.on('click', function () {
        navbarLinks.removeClass('active');
        $(this).addClass('active');
    });
});
