let book = document.getElementById('bookTable');

function checkBooks() {
    if (book.rows.length === 1) {
        alert('Please Select Book');
        return false;
    }
    return true;
}