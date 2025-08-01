function DeleteImageCompleted(data, id) {
    if (data == "True") {
        $("#image_" + id).fadeOut('slow');

        RefreshImages();

    } else {
        Swal.fire("پاک نشد!", "این عکس، استفاده شده است و نمی توانید آن را پاک کنید", "error");
    }
}
function RefreshImages() {
    $('#LoadingGallery').show();
    $.ajax({
        url: "/Admin/Media/GetFirstImages",
        type: "GET",
    })
        .done(function (partialViewResult) {
            $("#imageGallery").html(partialViewResult);
            $('#LoadingGallery').hide();
        });

    pageNumber = 2;
}

Dropzone.options.myAwesomeDropzone = {
    init: function () {
        this.on("success", function (file) {
            RefreshImages();
            setTimeout(function () { RemovePreviewDropzone(file) }, 3000);

        });

    }
};

function RemovePreviewDropzone(file) {
    file.previewElement.parentNode.removeChild(file.previewElement);
    $("#my-awesome-dropzone").removeClass("dz-started");

}


function img_postgallery_clicked(image) {
    if (cntrlIsPressed) {
        if ($(image).hasClass("img_postgallery_selected")) {
            $(image).removeClass("img_postgallery_selected");
        }
        else {
            $(image).addClass("img_postgallery_selected");
        }
    }
    else {

        $(".img_postgallery_selected").removeClass("img_postgallery_selected");
        $(image).addClass("img_postgallery_selected");
    }
}

var cntrlIsPressed = false;

$(document).keydown(function (event) {
    if (event.which == "17")
        if (isThumb != 1)
            cntrlIsPressed = true;
});

$(document).keyup(function () {
    cntrlIsPressed = false;
});


$(".insertImage").click(function () {
    if (isThumb == 0) {
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_url = $(obj).css("background-image");
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");

            $('#mainText').summernote('restoreRange');
            var imgNode = document.createElement("img");
            imgNode.src = img_url;
            $("#mainText").summernote("insertNode", imgNode);
        });
    }
    else if (isThumb == 1) {
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_id = $(obj).attr('id');
            var img_url = $(obj).css("background-image");
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");
            $('#thumb_url').val(img_id);
            $('#Show_Thmub').css('background-image', 'url(' + img_url + ')');
        });
    } else if (isThumb == 2){
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_url = $(obj).css("background-image");
            var img_id = $(obj).attr('id');
            img_id = img_id.replace('image_', '');
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");

            $(".productImageGalleries").append("<div class=\"productImageGallery\" style=\"background-image: url('" + img_url + "  ');\"><input type=\"hidden\" name=\"ProductImageGallery\" value=\"" + img_id + "\" /><a class=\"delGalleryImgBtn\" id=\"" + img_id + "\"><i class=\"fad fa-trash-alt\"></i></a></div > ");
        });
    } else if (isThumb == 3) {
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_id = $(obj).attr('id');
            var img_url = $(obj).css("background-image");
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");
            $('#banner1Img_url').val(img_id);
            //$('#Show_banner1Img').css('background-image', 'url(' + img_url + ')');
            $('#Show_banner1Img img').attr("src", img_url);
        });
    } else if (isThumb == 4) {
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_id = $(obj).attr('id');
            var img_url = $(obj).css("background-image");
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");
            $('#banner2Img_url').val(img_id);
            //$('#Show_banner2Img').css('background-image', 'url(' + img_url + ')');
            $('#Show_banner2Img img').attr("src", img_url);
        });
    } else if (isThumb == 5) {
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_id = $(obj).attr('id');
            var img_url = $(obj).css("background-image");
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");
            $('#banner3Img_url').val(img_id);
            //$('#Show_banner3Img').css('background-image', 'url(' + img_url + ')');
            $('#Show_banner3Img img').attr("src",img_url);
        });
    } else if (isThumb == 6) {
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_id = $(obj).attr('id');
            var img_url = $(obj).css("background-image");
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");
            $('#banner4Img_url').val(img_id);
            //$('#Show_banner4Img').css('background-image', 'url(' + img_url + ')');
            $('#Show_banner4Img img').attr("src", img_url);
        });
    } else if (isThumb == 7) {
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_id = $(obj).attr('id');
            var img_url = $(obj).css("background-image");
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");
            $('#mobileThumb_url').val(img_id);
            $('#Show_MobileThmub').css('background-image', 'url(' + img_url + ')');
        });
    } else if (isThumb == 8) {
        $('.img_postgallery_selected').each(function (i, obj) {
            var img_id = $(obj).attr('id');
            var img_url = $(obj).css("background-image");
            img_url = img_url.replace('url(', '').replace(')', '').replace(/\"/gi, "");
            $('#bannerWonderImg_url').val(img_id);
            //$('#Show_bannerWonderImg').css('background-image', 'url(' + img_url + ')');
            $('#Show_bannerWonderImg img').attr("src", img_url);
        });
    }
});

$(".deleteImage").click(function () {

    $('#LoadingGallery').show();

    var AllId = [];
    $('.img_postgallery_selected').each(function (i, obj) {
        var img_id = $(obj).attr('id').split("_")[1];
        AllId.push(img_id);

    });
    debugger;
    $.ajax({
        url: "/Admin/Media/DeleteManyImages",
        type: "POST",
        data: { AllImageId: AllId },
    }).done(function (result) {
        if (result == true) {
            Swal.fire("موفق", "تمام عکس های انتخابی حذف شدند", "success");
        }
        else {
            Swal.fire("هشدار", "برخی از عکس ها حذف نشدند", "warning");
        }
        RefreshImages();
    });
});

var isThumb = 1;


function OpenGallery(BtnText, number) {
    $(".insertImage").html(BtnText);
    isThumb = number;
    RefreshImages();
}
$("#btn_Gallery").click(function () {
    $(".insertImage").html("قرار بده!");
    $('#mainText').summernote('saveRange');
    isThumb = 0;
    RefreshImages();

});

$("#btn_Gallery_thumb").click(function () {
    isThumb = 1;
    $(".insertImage").html("انتخاب تصویر شاخص");
    RefreshImages();
});

$("#btn_product_gallery").click(function () {
    $(".insertImage").html("انتخاب عکس");
    $('#mainText').summernote('saveRange');
    isThumb = 2;
    RefreshImages();
});

$("#btn_Gallery_banner1Img").click(function () {
    $(".insertImage").html("انتخاب عکس");
    isThumb = 3;
    RefreshImages();
});

$("#btn_Gallery_banner2Img").click(function () {
    $(".insertImage").html("انتخاب عکس");
    isThumb = 4;
    RefreshImages();
});

$("#btn_Gallery_banner3Img").click(function () {
    $(".insertImage").html("انتخاب عکس");
    isThumb = 5;
    RefreshImages();
});

$("#btn_Gallery_banner4Img").click(function () {
    $(".insertImage").html("انتخاب عکس");
    isThumb = 6;
    RefreshImages();
});

$("#btn_Gallery_bannerWonderImg").click(function () {
    $(".insertImage").html("انتخاب عکس");
    isThumb = 8;
    RefreshImages();
});

$("#btn_Gallery_mobileThumb").click(function () {
    isThumb = 7;
    $(".insertImage").html("انتخاب تصویر شاخص");
    RefreshImages();
});


var pageNumber = 2;
$("#WrapGallery").on("scroll", function () {
    var scrollHeight = $(this)[0].scrollHeight;
    var scrollPosition = $("#WrapGallery").innerHeight() + $("#WrapGallery").scrollTop();

    if ((scrollHeight - scrollPosition) === 0) {

        console.log("done");
        GetMoreImages(pageNumber);
    }

});

function GetMoreImages(page) {
    $('#LoadingGallery').show();
    $.ajax({
        url: "/Admin/Media/GetMoreImages",
        type: "GET",
        data: { page: page },
    }).done(function (partialViewResult) {
        $("#imageGallery").append(partialViewResult);
        $('#LoadingGallery').hide();
        pageNumber = pageNumber + 1;
    });
}
