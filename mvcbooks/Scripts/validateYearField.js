const validateMessage = document.querySelector('#validateMessage');
const submit = document.getElementById('submit');

const validateYear = (e) => {
    
    let yearVal = document.querySelector('#year').value;

    var currentYear = new Date().getFullYear();

    if (yearVal.indexOf('.') > -1 || yearVal.indexOf('-') > -1 || yearVal.indexOf('+') > -1) {
        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "Insert only integers!"
        return false;
    }

    if (yearVal > currentYear) {

        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "Year should be in range from 1 to current year!"
        return false;
    }

    // check if date is starting with zero or not
    if (yearVal[0] == 0) {
        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "Date can't be started with zero!"
        return false;
    }

    // check if date is negative or not
    if (yearVal < 0) {
        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "No minus sign!"
        return false;
    }


    validateMessage.style.display = 'none';
    return true;
}