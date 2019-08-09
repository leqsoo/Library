$(document).ready(function () {
    $(".row-color").each(function () {
        $(".left-quantity", this).each(function () { // iterates through a whole column
            if (parseFloat($(this).text()) === 0) {
                $(this).closest('tr').css('background', 'rgba(255,99,71, 0.8)');
            } else if ((parseFloat($(this).text())) / parseFloat($(this).next().val()) <= 1/4) { // gets those values which are equal or less than 25% of a total quantity
                $(this).closest('tr').css('background', 'rgba(255, 255, 51, 0.6)');
            } else if ((parseFloat($(this).text())) / parseFloat($(this).next().val()) <= 3/4) { // gets those values which are between 25% and 75% of a total quantity
                $(this).closest('tr').css('background', 'lightblue');
            } else { // gets those values which are greater than 75% of a total quantity
                $(this).closest('tr').css('background', 'rgba(153, 255, 51, 0.8)');
            }
        });
    });
});