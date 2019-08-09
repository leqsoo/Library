$(document).ready(function () { //when page loads
    let arr = [];
    let datesArr = [] = new Array();
    let k = 0;

    function Dates(id, dateFrom, dateTo) {
        this.id = id;
        this.dateFrom = dateFrom;
        this.dateTo = dateTo;
    }

    $("#addBookBtn").click(function () {
        {
            let selected = $('.booksSelect :selected'); //select books table 

            selected.each(function (a) {
                k++;
                //datesArr[k - 1] = new Array(3).push($(this).val(), "", "");
                datesArr[k - 1] = new Dates($(this).val(), "", "");

                $('#bookTable').append(`<tr><td class='bookId'>${$(this).val()}</td><td class='book'>${$(this).text()}
                    <button class="delete btn btn-danger btn-sm float-right">x</button></td>                                
                    <td><input id="start" class="borrowFrom" type="date" name="borrowFrom" placeholder="Borrow From"></td>
                    <td><input id="end" required class="borrowTo" type="date" name="borrowTo" placeholder="Borrow From"></td></tr>`);                
                //add table body

                //$('#hiddenInputForDates').append(datesArr[k - 1].id + "_" + datesArr[k - 1].dateFrom + "_" + datesArr[k - 1].dateTo);
            });
            $("#start").rules("add", {
                required: true,
                messages: {
                    required: "Start Date is required"
                }
            });$('#end').rules("add", {
                required: true,
                messages: {
                    required: "End Date is required"
                }
            });
            $('.bookId').each(function () {
                arr.push($(this).text()); //add selected options to array
            });

            console.table(datesArr);
            //console.log(datesArr[datesArr.length - 1].id);

            $('#hiddenInputForBookId').val(arr.join(',')); //add id's into hidden field
            $('.booksSelect option:selected').remove();
            arr = [];
        }
    });
    $("#start").rules("add", {
        required: true,
        messages: {
            required: "Required input"
        }
    });

    $('body').on('change', '.borrowFrom', function () {
        console.log($(this).val());
        //console.log(Number.parseInt($(this).closest("tr").after().text()))
        let myId = Number.parseInt($(this).closest("tr").after().text());
        for (let i = 0; i < datesArr.length; i++) {
            if (datesArr[i].id == myId) {
                datesArr[i] = new Dates(JSON.stringify(myId), $(this).val(), "");
                console.table(datesArr);
            }
        }
    })
    $('body').on('change', '.borrowTo', function () {
        console.log($(this).val());
        //console.log(Number.parseInt($(this).closest("tr").after().text()))
        let myId = Number.parseInt($(this).closest("tr").after().text());
        for (let i = 0; i < datesArr.length; i++) {
            if (datesArr[i].id == myId) {
                datesArr[i] = new Dates(JSON.stringify(myId), "", $(this).val());
                console.table(datesArr);
            }
        }
    })

    $('#submit').on('click', function () {
        $('#hiddenInputForDates').removeAttr('value');
        for (let i = 0; i < datesArr.length; i++) {
            if (datesArr[i].id !== "") {
                $('#hiddenInputForDates').val($('#hiddenInputForDates').val() + "," + datesArr[i].id + "_" + datesArr[i].dateFrom + "_" + datesArr[i].dateTo);
                //  $('#hiddenInputForDates').val($('#hiddenInputForDates').val.substring(1, $('#hiddenInputForDates').length));
            }
            else {
                alert("fill");
            }
        }
    })

    $("#bookTable").on('click', 'button.delete', function () {
        let tableRows = $(this).closest('tr');
        let deletedElementId = $(this).closest('td').prev().html();
        tableRows.remove();
        let myId = Number.parseInt($(this).closest("tr").after().text());
        for (let i = 0; i < datesArr.length; i++) {
            if (datesArr[i].id == myId) {
                datesArr[i].id = "";
                console.table(datesArr);
                break;
            }
        }
        
        let bookIdArr = $('#hiddenInputForBookId').val().split(',');

        bookIdArr.splice($.inArray(deletedElementId, bookIdArr), 1);  //remove deleted element from array

        $("#hiddenInputForBookId").val(bookIdArr.join(',')); //update hidden field when delete

        let book = $(this).closest('td.book')
        $(".booksSelect").append(`<option value="${deletedElementId}">${book.html().replace('x', '')}</option>`); //add option back to select
        $('.booksSelect').html($('.booksSelect').find('option').sort(function (x, y) {
            // to change to descending order switch "<" for ">"
            return $(x).text() > $(y).text() ? 1 : -1;
        }));
        $(".booksSelect").val($(".booksSelect option:first").val());
    });

    
        
   
});
