function validateForm() {
    var a = document.forms["Form"]["Message"].value;
    if (a == null || a == "") {
        alert("Please fill in all required fields");
        return false;
    }
    return true;
}