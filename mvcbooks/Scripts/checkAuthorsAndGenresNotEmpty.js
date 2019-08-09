let author = document.getElementById('authorTable');
let genre = document.getElementById('genreTable');

function checkAuthorsAndGenres() {
    if (author.rows.length === 1) {
        alert('Please Select Author');
        return false;
    }
    else if (genre.rows.length === 1) {
        alert('Please select Genre');
        return false;
    }
    else {
        return true;
    }
}