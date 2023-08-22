(function () {
    //add address tag
    const addOption = function (id, address) {
        let select = $(".addresses-palce");

        let addressAdder = $([
            "<div class='border mx-2 p-2 rounded address-row' style='min-width: 100px; height: fit-content; width: fit-content; display:inline'>",
            "    <p class='m-0 address-name'>" + address +"</p>",
            "    <p class='btn btn-outline-danger m-0 p-0 w-100 ' delete='" +id+"'>Видалити</p>",
            "</div>",
        ].join("\n"));

        $(select).append(addressAdder);
    }

    //send delete address request
    const deteleAddress = function (id, JQview) {
        $.ajax({
            url: '/Account/DeleteAdress/',
            data: { id: id },
            type: 'POST',
        }).done(function (data) {
            showMsg(data.Massage);
            JQview.remove();
        }).fail(function (data) {
            if (data.Massage) showMsg(data.Massage);
            else showMsg("Помилка видалення!")
        });
    }

    //show delete confirm msg
    const deleteConfirm = function (jqelement, addressRow) {
        let id = jqelement.attr("delete");
        let name = addressRow.find(".address-name").text();
        let msg = "Ви бажаєте видалити \"" + name + "\"?"

        confirmMsg(msg, () => deteleAddress(id, addressRow));
    }

    //add events
    $(".addresses-palce").on("click", function (e) {
        let elemet = $(e.target);
        if (!elemet.attr("delete")) return;

        checkAccount(() => deleteConfirm(elemet, elemet.closest(".address-row")));
    })

    $(".create-address").on("click", function () {
        showAdressCreate(addOption);
    });
})()