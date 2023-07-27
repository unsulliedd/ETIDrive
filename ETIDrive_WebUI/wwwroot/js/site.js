function showDeleteModal(itemName, deleteUrl) {
    const modal = $(`[class="modal delete-modal"]`).first();
    modal.find(".modal-body span").text(itemName);
    modal.modal('show');

    modal.find("#confirmDeleteButton").click(function () {
        window.location.href = deleteUrl;
    });
}

$(".close-button").click(function () {
    $(this).closest('.modal').modal('toggle');
});