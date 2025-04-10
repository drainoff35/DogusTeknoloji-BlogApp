// Genel silme işlemi için
function setupDeleteConfirmation(entityName, successCallback) {
    $(document).ready(function () {
        // Silme butonuna tıklanınca
        $(`.delete-${entityName}`).click(function () {
            const id = $(this).data('id');
            const itemTitle = $(this).data(`${entityName.toLowerCase()}-title`) ||
                $(this).data('title') ||
                $(this).data('comment-text') || '';

            // Modal göster
            const deleteModal = new bootstrap.Modal(document.getElementById(`deleteConfirmModal-${entityName}-${id}`));
            deleteModal.show();
        });

        // Silme onayı
        $(`.confirmDeleteBtn-${entityName}`).click(function () {
            const id = $(this).data('id');
            const url = $(this).data('url');
            console.log(`Attempting to delete ${entityName} with ID: ${id}, URL: ${url}`);

            // Form verisi oluştur
            const formData = new FormData();

            // AntiForgeryToken ekle
            const token = $('input[name="__RequestVerificationToken"]').val();
            if (token) {
                formData.append("__RequestVerificationToken", token);
            }

            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (result) {
                    console.log("Result:", result);
                    if (result && result.success) {
                        if (typeof successCallback === 'function') {
                            successCallback(result);
                        } else {
                            location.reload(); // Varsayılan - sayfayı yenile
                        }
                    } else {
                        alert(result && result.message ? result.message : 'İşlem sırasında bir hata oluştu.');
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Hata:", xhr, status, error);
                    alert(`${entityName} silinirken bir hata oluştu.`);
                }
            });
        });
    });
}
