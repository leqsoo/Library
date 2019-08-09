$(document).ready(function () {
    $('.fa-dfrom').click(function () {
        $('.dateFromDropDown').toggle();
    });

    $('.dateFromDropDown span').click(function () {

        $('.dateFromInput').val($(this).text()); //insert value into input
        $('.yearContainer').hide();
    });

    $('.fa-dto').click(function () {
        $('.dateToDropDown').toggle();
    });

    $('.dateToDropDown span').click(function () {

        $('.dateToInput').val($(this).text()); //insert value into input
        $('.yearContainer').hide();
    });

    var mouse_is_inside = false;

    $(document).ready(function () { 
        $('.yearContainer').hover(function () {
            mouse_is_inside = true;
        }, function () {
            mouse_is_inside = false;
        });

        $("body").mouseup(function () {
            if (!mouse_is_inside) $('.yearContainer').hide();
        });
    });
    
    $(function () {
        let years = []; //empty array for years
        let currentYear = new Date().getFullYear();
        for (let i = 1; i <= currentYear; i++) {
            years.push({ value: i, id: i }); //fill array with years
        }
        $(".dateFromInput").autocomplete({
            source: years,
            minLength: 2 //display options after length is 2

        });
        $(".dateToInput").autocomplete({
            source: years,
            minLength: 2 //display options after length is 2

        });
    });
});
