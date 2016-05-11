/* some common functions */
function tourl(url) {
	window.location.href = url;
}

function displayLoadingStatus(display) {
	if (display) {
		$('.loading-status').show();
	}
	else {
		$('.loading-status').hide('slow');
	}
}

function displayNotificationBar(message, messagetype) {

    // clear previous state
    $('#notification-bar')
        .removeClass('alert-warning')
        .removeClass('alert-success');
    $("#notification-bar .content").html("");

    // css
    var cssclass = 'alert-success';
    if (messagetype == 'error') {
        cssclass = 'alert-warning';
    }
    else {
        cssclass = 'alert-success'
    }

    // notifications
	$("#notification-bar .content").html(message);
	$('#notification-bar').addClass(cssclass).fadeIn("slow").delay(3500).fadeOut("slow");
}

/* Ajax for add to shoppingcart */
var AjaxAddtoCart = {
    cartselector: '',
    wishlistselector: '',

    init: function (cartselector, wishlistselector) {
        this.cartselector = cartselector;
        this.wishlistselector = wishlistselector;
    },

	showLoadWaiting: function () {
		displayLoadingStatus(true);
	},

	addproducttocart: function (urladd) {
		this.showLoadWaiting();

		$.ajax({
			cache: false,
			url: urladd,
			type: 'post',
			success: this.success_process,
			complete: this.closeLoadWaiting,
			error: this.ajaxFailure
		});
	},

	addproducttocart_detail: function (urladd, strquantity) {
		this.showLoadWaiting();

		$.ajax({
			cache: false,
			url: urladd,
			data: { "strquantity": strquantity },
			type: 'post',
			success: this.success_process,
			complete: this.closeLoadWaiting,
			error: this.ajaxFailure
		});
	},

	closeLoadWaiting: function () {
	    displayLoadingStatus(false);
	},

	success_process: function (response) {
	    if (response.updatecartinfo) {
	        $(AjaxAddtoCart.cartselector).html(response.updatecartinfo);
	    }
	    if (response.updatewishlistinfo) {
	        $(AjaxAddtoCart.wishlistselector).html(response.updatewishlistinfo);
	    }

	    if (response.message) {
	        if (response.success == true) {
	            displayNotificationBar(response.message, 'success');
	        }
	        else {
	            displayNotificationBar(response.message, 'error');
	        }
	        return false;
	    }
	    return false;
	},

	ajaxFailure: function () {
	    alert('Failed to add the product to the cart. Please try again.');
	}
};