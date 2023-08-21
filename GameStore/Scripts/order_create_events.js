(function () {
    const addOption = function (id, address) {
        $("#Addresses").append($("<option value='" + id + "'>" + address + "</option>"))
    }
    
    $(".create-address").on("click", function () {
        showAdressCreate(addOption);
    });
})()