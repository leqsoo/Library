$(document).ready(function () { //when page loads
    let arr = [];
   
    $("#addGenreBtn").click(function () {
        {
            let selected = $('.genresSelect :selected'); //select genres table 

            selected.each(function (a) {
                $('#genreTable').append(`<tr><td class='genreId'>${$(this).val()}</td><td class='genre'>${$(this)
                    .text()}<button class="delete btn btn-danger btn-sm float-right">x</button></td></tr>`);
                //add table body
            });

            $('.genreId').each(function () {
                arr.push($(this).text()); //add selected options to array

            });

            $('#hiddenInputForGenreId').val(arr.join(',')); //add id's into hidden field
            $('.genresSelect option:selected').remove();
            arr = [];
        }
    });

    $("#genreTable").on('click', 'button.delete', function () {
        let tableRows = $(this).closest('tr');
        let deletedElementId = $(this).closest('td').prev().html();
        tableRows.remove();

        let genreIdArr = $('#hiddenInputForGenreId').val().split(',');

        genreIdArr.splice($.inArray(deletedElementId, genreIdArr), 1); //remove deleted element from array

        $("#hiddenInputForGenreId").val(genreIdArr.join(',')); //update hidden field when delete

        let genre = $(this).closest('td.genre');
        $(".genresSelect").append(`<option value="${deletedElementId}">${genre.html().replace('x', '')}</option>`); //add option back to select


    });
});
