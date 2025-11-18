// Hàm này dùng để thêm Token bảo mật vào mỗi lần gọi AJAX POST
function addAntiForgeryToken(data) {
    // Tìm Token trong file Index.cshtml
    data.__RequestVerificationToken = $('input[name="__RequestVerificationToken"]').val();
    return data;
}

// Hàm này dùng để cập nhật cái giỏ hàng "mini" trên Header
function updateMiniCart() {
    $.ajax({
        url: '/Cart/GetCartSummaryJson',
        type: 'GET',
        success: function (data) {
            $('#cart-count').text(data.count); // Cập nhật số lượng
            $('#cart-total').text(data.total + ' ₫'); // Cập nhật tổng tiền
        }
    });
}


$(document).ready(function () {

    // 1. NÚT THÊM VÀO GIỎ (trên trang sản phẩm)
    // Giả sử nút của bà có class là "btn-add-to-cart"
    $('.btn-add-to-cart').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('id');

        var data = { id: productId };

        // Gọi AJAX (POST)
        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: addAntiForgeryToken(data), // Thêm Token
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    updateMiniCart(); // Cập nhật giỏ hàng "mini"
                } else {
                    alert(response.message); // Báo lỗi (VD: hết hàng)
                }
            }
        });
    });

    // 2. NÚT CẬP NHẬT SỐ LƯỢNG (trong trang Index.cshtml)
    // Gõ vào ô số lượng và thả chuột ra
    $('.btn-update-cart').on('change', function () {
        var productId = $(this).data('id');
        var newQuantity = $(this).val();

        var data = {
            id: productId,
            quantity: newQuantity
        };

        $.ajax({
            url: '/Cart/UpdateCart',
            type: 'POST',
            data: addAntiForgeryToken(data),
            success: function (response) {
                if (response.success) {
                    // Cập nhật giá của dòng đó
                    $('#total_' + productId).text(response.itemTotal + ' ₫');
                    // Cập nhật tổng tiền
                    $('#cart-page-total').text(response.cartTotal + ' ₫');
                    updateMiniCart(); // Cập nhật giỏ hàng "mini"
                } else {
                    alert(response.message);
                    $(this).val(response.newQuantity); // Trả số lượng về cũ nếu lỗi
                }
            }
        });
    });

    // 3. NÚT XOÁ 1 MÓN (trong trang Index.cshtml)
    $('.btn-remove-cart').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('id');

        if (confirm('Bạn có chắc muốn xóa sản phẩm này?')) {
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'POST',
                data: addAntiForgeryToken({ id: productId }),
                success: function (response) {
                    if (response.success) {
                        // Xoá dòng đó khỏi table
                        $('#row_' + productId).remove();
                        // Cập nhật tổng tiền
                        $('#cart-page-total').text(response.cartTotal + ' ₫');
                        updateMiniCart(); // Cập nhật giỏ hàng "mini"
                    }
                }
            });
        }
    });

    // 4. NÚT XOÁ SẠCH (trong trang Index.cshtml)
    $('.btn-clear-cart').on('click', function (e) {
        e.preventDefault();

        if (confirm('Bạn có chắc muốn xóa TOÀN BỘ giỏ hàng?')) {
            $.ajax({
                url: '/Cart/ClearCart',
                type: 'POST',
                data: addAntiForgeryToken({}),
                success: function (response) {
                    if (response.success) {
                        // Xoá hết bảng
                        $('#cart-table tbody').empty();
                        // Reset tổng tiền
                        $('#cart-page-total').text('0 ₫');
                        updateMiniCart(); // Cập nhật giỏ hàng "mini"
                    }
                }
            });
        }
    });
});