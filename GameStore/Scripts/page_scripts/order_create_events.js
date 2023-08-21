(function () {
    const addOption = function (id, address) {
        let select = $("#Address");
        select.append($("<option value='" + address + "'>" + address + "</option>"))
        select.get()[0].value = address;

    }
    
    $(".create-address").on("click", function () {
        showAdressCreate(addOption);
    });
})()