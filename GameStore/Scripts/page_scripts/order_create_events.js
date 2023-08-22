(function () {
    //add option to list and choose it
    const addOption = function (id, address) {
        let select = $("#Address");
        select.append($("<option value='" + address + "'>" + address + "</option>"))
        select.get()[0].value = address;

    }

    //add event
    $(".create-address").on("click", function () {
        showAdressCreate(addOption);
    });
})()