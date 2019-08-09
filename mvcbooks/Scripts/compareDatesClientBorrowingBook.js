//$(document).ready(function () {    
//    console.log($('#datepickerFrom').val());
//    console.log($('#datepickerTo').val());
//    $("#datepickerFrom").datepicker({
//        dateFormat: "dd/MM/yy",
//        changeMonth: true, //enable change month by dropdown
//        changeYear: true, //enable change year by dropdown
//        yearRange: new Date().getFullYear() + ':c+10', //year range is from current year to current year + 10 
//        minDate: 0, //can't select before today
//        //restrict date selection depending on other datepicker
//        numberOfMonth: 1,

//        onSelect: function (selected) {
//            $("#datepickerTo").datepicker("option", "minDate", selected)
//        }
//    });
//    $("#datepickerTo").datepicker({
//        dateFormat: "dd/MM/yy",
//        changeMonth: true, //enable change month by dropdown
//        changeYear: true, //enable change year by dropdown
//        yearRange: new Date().getFullYear() + ':c+10', //year range is from current year to current year + 10 
//        minDate: 0, //can't select before today
//        //restrict date selection depending on other datepicker


//        onSelect: function (selected) {
//            $("#datepickerFrom").datepicker("option", "maxDate", selected)
//        }
//    });
//});
function compareDates() {
    //Get elements
    let from = document.getElementById('datepickerFrom').value;
    let to = document.getElementById('datepickerTo').value;
    let errorMessage = document.getElementById('dateComparisonValidationMessage');
    let startDate = new Date(from);
    ;
    //start date and end date comparison logic
    if (to == null) {
        errorMessage.style.display = "none";
        return true;
    }
    else {
        let endDate = new Date(to);
        if (startDate > endDate) {
            errorMessage.innerHTML = "End Date is before the Start Date! We have no time machines here!";
            errorMessage.style.display = "block";
            return false;
        }
        errorMessage.style.display = "none";
        return true;
    }
}