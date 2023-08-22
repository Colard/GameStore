(function () {

    'use strict';

    //set event settings
    const preventDefaults = event => {
        event.preventDefault();
        event.stopPropagation();
    };

    const highlight = e => $(e.target).addClass('highlight');

    const unhighlight = e => $(e.target).addClass('highlight');

    //get all "drag and drop" elements
    const getInputAndGalleryRefs = element => {
        const zone = element.closest('.upload_dropZone') || false;
        const gallery = document.querySelector('.upload_gallery') || false;
        const input = zone.querySelector('input[type="file"]') || false;
        return { input: input, gallery: gallery };
    }

    const handleDrop = event => {
        const dataRefs = getInputAndGalleryRefs(event.target);
        dataRefs.files = event.dataTransfer.files;
        handleFiles(dataRefs);
    }

    //event for choose main photo
    const chooseMainImage = e => {
        let currentElement = e.target;
        if (currentElement.tagName != "IMG") return;

        currentElement = $(currentElement);
        let parent = currentElement.parent(".gallery_cheked_img");
        let grandParent = parent.parent();

        grandParent.children().removeClass("selected_img");

        parent.addClass("selected_img");

        let file = parent.find("input[name='imageFile']").val();
        grandParent.find(".main_img_container").val(file);
    }

    //add events to zone
    const eventHandlers = zone => {

        const dataRefs = getInputAndGalleryRefs(zone);
        if (!dataRefs.input) return;

        ;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(event => {
            zone.addEventListener(event, preventDefaults, false);
            document.body.addEventListener(event, preventDefaults, false);
        });

        ;['dragenter', 'dragover'].forEach(event => {
            zone.addEventListener(event, highlight, false);
        });
        ;['dragleave', 'drop'].forEach(event => {
            zone.addEventListener(event, unhighlight, false);
        });

        $(dataRefs.gallery).on("click", chooseMainImage);
        zone.addEventListener('drop', handleDrop, false);

        dataRefs.input.addEventListener('change', event => {
            dataRefs.files = event.target.files;
            handleFiles(dataRefs);
        }, false);

    }

    //add event to drop zones
    const dropZones = document.querySelectorAll('.upload_dropZone');
    for (const zone of dropZones) {
        eventHandlers(zone);
    }

    //check file format
    const isImageFile = file => ['image/jpeg', 'image/png'].includes(file.type);

    //show img
    function previewFiles(dataRefs) {
        if (!dataRefs.gallery) return;
        for (const file of dataRefs.files) {
            let reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onloadend = function () {
                let checkedImg = $([
                    "<div class='gallery_cheked_img mx-1'>",
                    "  <img class='upload_img' alt = '" + file.name + "' src= '" + reader.result + "' >",
                    "</div>"
                ].join("\n"));

                let closeSpan = $("<p class='close_span'>&#10060</p>")
                    .on("click", function () { $(this).parent().remove(); })

                let imageContainerInput = $("<input class='unvisible_checkbox' name='imageFile' value = '" + reader.result + "'>");

                checkedImg.append(closeSpan);
                checkedImg.append(imageContainerInput);

                $(dataRefs.gallery).append(checkedImg);

            }
        }
    }

    const handleFiles = dataRefs => {

        let files = [...dataRefs.files];

        files = files.filter(item => isImageFile(item) ? item : null);

        if (!files.length) return;
        dataRefs.files = files;

        previewFiles(dataRefs);
    }

})();