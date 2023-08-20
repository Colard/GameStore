(function () {
    "use strict"

    //show buy panel
    const showBuyPanel = function (jq_product) {
        let product = jq_product;
        let card = product.closest(".card");
        let img = card.find("img.good-img");
        let name = card.closest(".card").find(".good-name").text();

        let buyPanel = $([
            "<div class='d-flex justify-content-center align-items-center content-blocker'>",
            "    <div class='max-width: 600px; text-center p-5' style='border: 1px solid #ccc; border-radius: 10px; background-color: #FFFFFF;'>",
            "        <div class='aditional-info-container'>",
            "        </div>",
            "        <h6 class = 'good-count'>Введіть кількість:</h6>",
            "        <input type='number' style='display: inline;' class='form-control good-count-input' placeholder='Max. 20' min = '1' max = '20' value = '1'>",
            "        <h6 class='error-msg'><h6>",
            "        <div class='wrapper row  py-2'>",
            "            <div class='col-md-6'>",
            "                <p class='btn btn-outline-danger w-100'>Закрити</p>",
            "            </div>",
            "            <div class='col-md-6'>",
            "                <p class='btn btn-outline-dark w-100 bye-btn'>Придбати</p>",
            "            </div>",
            "        </div>",
            "    </div>",
            "</div>",
        ].join("\n"));

        buyPanel.find("div.aditional-info-container").append(img.clone().removeClass().addClass("good_card"));
        buyPanel.find("div.aditional-info-container").append($("<h5></h5>").addClass("fw-bolder").text(name));
        buyPanel.find(".btn-outline-danger").on("click", function () {
            buyPanel.remove();
        });

        buyPanel.find(".bye-btn").on("click", function () {
            addToCart(product, buyPanel);
        });

        $(".body-content").append(buyPanel);
    }

    //add error msg to buyBanel
    const showErrorMsg = function(buyPanel, errorMsg) {
        buyPanel.find(".error-msg").text(errorMsg);
    }

    //add product to cart
    const addToCart = function(jq_product, buyPanel) {

        let productId = jq_product.attr("product");
        let count = buyPanel.find(".good-count-input").val();

        let order = {
            ProductId: productId,
            Count: count,
        };

        $.ajax({
            type: "POST",
            url: "/Cart/AddToCart",
            data: '{cart: ' + JSON.stringify(order) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
        }).done(function (data) {
            let response = data;

            if (response.ResponseType == "success") {
                console.log(data.Massage)
                showMsg(data.Massage);
                buyPanel.remove();
            }

            if (response.ResponseType == "error") {
                showErrorMsg(buyPanel, data.Massage)
            }
        })

    };

    //view cart info
    $("[product]").on("click", function (e) {
        let product = $(this);
        checkAccount(() => showBuyPanel(product));
    });
}
)()