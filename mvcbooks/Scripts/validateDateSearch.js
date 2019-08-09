const validateMessage = document.querySelector('#validateMessage');
const submit = document.getElementById('submit');

const validate = (e) => {
    
    let dateFromVal = document.querySelector('#dateFrom').value;
    let dateToVal = document.querySelector('#dateTo').value;
    

    var currentYear = new Date().getFullYear();

    if (dateFromVal.indexOf('.') > -1 || dateFromVal.indexOf('-') > -1 || dateFromVal.indexOf('+') > -1) {
        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "Insert only integers!"
        return false;
    }

    if (dateToVal.indexOf('.') > -1 || dateToVal.indexOf('-') > -1 || dateToVal.indexOf('+') > -1) {
        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "Insert only integers!"
        return false;
    }

    if (dateToVal > currentYear || dateFromVal > currentYear) {

        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "Year should be in range from 1 to current year!"
        return false;
    }

    // check if start date is less than end
    if ((parseInt(dateFromVal) >= parseInt(dateToVal)) && dateToVal != '') {
        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "Starting date must be less than last!"
        return false;
    }
    
    // check if date is starting with zero or not
    if (dateFromVal[0] == 0 || dateToVal[0] == 0) {
        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "Date can't be started with zero!"
        return false;
    }

    // check if date is negative or not
    if (dateFromVal < 0 || dateToVal < 0) {
        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "No minus sign!"
        return false;
    }


    validateMessage.style.display = 'none';
    return true;
}