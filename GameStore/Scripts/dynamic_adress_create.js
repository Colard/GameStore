
//show addressAdder and in success address create run callback
//callback take id and text
const showAdressCreate = function (callback) {
    let addressAdder = $([
        "<div class='d-flex justify-content-center align-items-center content-blocker'>",
        "    <div class='max-width: 600px; text-center p-5 py-3' style='border: 1px solid #ccc; border-radius: 10px; background-color: #FFFFFF;'>",
        "        <form class = 'text-center'>",
        "        <h4>Додати адресу</h4>",
        "        <h6>Введіть адресу:</h6>",
        "        <input style='display: inline;' class='form-control' name = 'Address'>",
        "        <h6>Введіть поштовий індекс:</h6>",
        "        <input style='display: inline;' class='form-control' name = 'PostCode' >",
        "        <h6 class='error-msg'><h6>",
        "        </form>",
        "        <div class='wrapper row align-items-center py-2'>",
        "            <div class='col-md-6'>",
        "                <p class='btn btn-outline-danger w-100 m-0'>Закрити</p>",
        "            </div>",
        "            <div class='col-md-6'>",
        "                <p class='btn btn-outline-dark w-100 add-btn m-0'>Додати</p>",
        "            </div>",
        "        </div>",
        "    </div>",
        "</div>",
    ].join("\n"));

    addressAdder.find(".btn-outline-danger").on("click", function () {
        addressAdder.remove();
    });

    addressAdder.find(".add-btn").on("click", function () {
        addAddress(addressAdder, callback);
    });

    $(".body-content").append(addressAdder);
}

//add product to cart
const addAddress = function (addressAdder, callback) {

    let form = addressAdder.find("form");

    $.ajax({
        url: "/Account/CreateAdress",
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: form.serialize()
    }).done(function (data) {
        let response = data;

        if (response.ResponseType == "success") {
            showMsg("Адреса успішно додана!");
            addressAdder.remove();
            if (callback) callback(+data.Id, data.Massage);
        }

        if (response.ResponseType == "error") {
            showErrorMsg(data.Massage, data.Massage)
        }
    })

};

const showErrorMsg = function (addressAdder, errorMsg) {
    addressAdder.find(".error-msg").text(errorMsg);
}
