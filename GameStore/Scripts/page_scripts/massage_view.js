
//Show Msg
const showMsg = function (msg) {
    let msgPanel = $([
        "<div class='d-flex justify-content-center align-items-center content-blocker'>",
        "    <div class='text-center p-5' style='max-width: 600px; border: 1px solid #ccc; border-radius: 10px; background-color: #FFFFFF;'>",
        "        <h2 class = 'fw-bolder'>" + msg + "</h2>",
        "        <div class='wrapper row pt-3 justify-content-center'>",
        "            <div class='col-md-6'>",
        "                <p class='btn btn-outline-danger w-100'>Закрити</p>",
        "            </div>",
        "        </div>",
        "    </div>",
        "</div>",
    ].join("\n"));

    msgPanel.find(".btn-outline-danger").on("click", function () {
        msgPanel.remove();
    });

    $(".body-content").append(msgPanel);
}

//show confirm panel
const confirmMsg = function (msg, callback) {
    let msgPanel = $([
        "<div class='d-flex justify-content-center align-items-center content-blocker'>",
        "    <div class='text-center p-5' style='max-width: 600px; border: 1px solid #ccc; border-radius: 10px; background-color: #FFFFFF;'>",
        "        <h2 class = 'fw-bolder'>" + msg + "</h2>",
        "        <div class='wrapper row  py-2'>",
        "            <div class='col-md-6'>",
        "                <p class='btn btn-outline-danger w-100'>Закрити</p>",
        "            </div>",
        "            <div class='col-md-6'>",
        "                <p class='btn btn-outline-dark w-100 bye-btn'>Підтвердити</p>",
        "            </div>",
        "        </div>",
        "    </div>",
        "</div>",
    ].join("\n"));

    msgPanel.find(".btn-outline-danger").on("click", function () {
        msgPanel.remove();
    });

    msgPanel.find(".btn-outline-dark").on("click", function () {
        msgPanel.remove();
        if (callback) callback();
    });

    $(".body-content").append(msgPanel);
}