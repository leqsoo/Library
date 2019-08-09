const input = document.querySelector('.checkEmpty');
const validateMessage = document.querySelector('#validateMessage');

console.log(submit.value)
const validateForEmpty = () => {
    if (input.value === "") {
        console.log(2)

        validateMessage.style.display = 'block';
        validateMessage.innerHTML = "This Field is Required!"
        return false;
    }
    console.log(3)

    validateMessage.style.display = 'none';
    return true;
}