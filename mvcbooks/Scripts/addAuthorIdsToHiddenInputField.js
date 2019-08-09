$(document).ready(function () { //when page loads
    let arr = [];
    $("#addAuthorBtn").click(function () {
        {

            let selected = $('.authorsSelect :selected'); //select authors table 

            selected.each(function (a) {
                $('#authorTable').append(`<tr><td class='authorId'>${$(this).val()}</td><td class='author'>${$(this).text()}<button class="delete btn btn-danger btn-sm float-right">x</button></td></tr>`);
                //add table body
            });

            $('.authorId').each(function () {
                arr.push($(this).text()); //add selected options to array

            });

            $('#hiddenInputForAuthorId').val(arr.join(',')) //add id's into hidden field
            $('.authorsSelect option:selected').remove();
            arr = [];
            
        }
    });

    $("#authorTable").on('click', 'button.delete', function () {
        let tableRows = $(this).closest('tr');
        let deletedElementId = $(this).closest('td').prev().html();
        tableRows.remove();

        let authorIdArr = $('#hiddenInputForAuthorId').val().split(',');

        authorIdArr.splice($.inArray(deletedElementId, authorIdArr), 1);  //remove deleted element from array
        
        $("#hiddenInputForAuthorId").val(authorIdArr.join(',')); //update hidden field when delete

        let author = $(this).closest('td.author')
        $(".authorsSelect").append(`<option value="${deletedElementId}">${author.html().replace('x', '')}</option>`); //add option back to select


    });
});
