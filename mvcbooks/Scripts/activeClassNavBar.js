$(document).ready(function () {
    $('.navbar-nav').find('a[href="/"]')
        .closest('li').removeClass('active');
    $('.navbar-nav').find('a[href="' + location.pathname + '"]')
        .closest('li').addClass('active');
});



window.onscroll = function () {scrollFunction()};

function scrollFunction() {
    if (document.body.scrollTop > 70 || document.documentElement.scrollTop > 70) {
        document.querySelector(".navbar").style.padding = "15px 15px";
        document.querySelector(".logo").style.width = "32px";
        document.querySelector(".logo").style.height = "auto";
        document.querySelector("nav .captoin").style.fontSize = "16px";
    } else {
        document.querySelector(".navbar").style.padding = "30px 15px";
        document.querySelector(".logo").style.width = "100px";
        document.querySelector(".logo").style.height = "100px";
        document.querySelector("nav .captoin").style.fontSize = "25px";
    }
}
