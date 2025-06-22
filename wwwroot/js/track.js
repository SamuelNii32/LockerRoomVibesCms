document.addEventListener("DOMContentLoaded", function () {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });

    function cleanupModalBackdrop() {
        const backdrop = document.querySelector('.modal-backdrop');
        if (backdrop) backdrop.remove();
        document.body.classList.remove('modal-open');
        document.body.style.overflow = '';
    }



    const toastEl = document.getElementById("successToast");
    const toastBody = toastEl.querySelector(".toast-body");
    const successToast = new bootstrap.Toast(toastEl);

    // ===================== DELETE =====================
    const deleteModal = document.getElementById("deleteTrackModal");
    const deleteForm = deleteModal.querySelector("form");
    const deleteIdInput = deleteForm.querySelector("input[name='id']");
    const deleteModalBody = deleteModal.querySelector(".modal-body");
    const deleteBsModal = new bootstrap.Modal(deleteModal);

    window.prepareDeleteModal = (id, title) => {
        deleteIdInput.value = id;
        deleteModalBody.textContent = `Are you sure you want to delete "${title}"?`;
        deleteForm.action = `/Tracks/Delete/${id}`;
    };

    deleteForm.addEventListener("submit", (e) => {
        e.preventDefault();

        const formData = new FormData(deleteForm);
        fetch(deleteForm.action, {
            method: "POST",
            body: formData,
            headers: {
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(res => {
                if (!res.ok) throw new Error("Delete failed");
                return res.json();
            })
            .then(data => {
                if (!data.success) throw new Error(data.message || "Delete failed");
                deleteBsModal.hide();
                // Clean up modal backdrop and restore scroll
                cleanupModalBackdrop();

                const row = document.querySelector(`button[onclick*="prepareDeleteModal(${formData.get("id")}`)?.closest("tr");
                if (row) row.remove();

                toastBody.textContent = "Track deleted successfully!";
                successToast.show();
            })
            .catch(err => alert(err.message));
    });

    // ===================== EDIT =====================
    const editModal = document.getElementById("editTrackModal");
    const editForm = editModal.querySelector("form");
    const editBsModal = new bootstrap.Modal(editModal);

    window.populateEditModal = (id, title, artist, mood, audioUrl, duration) => {
        editForm.querySelector('input[name="Id"]').value = id;
        editForm.querySelector('input[name="Title"]').value = title;
        editForm.querySelector('input[name="Artist"]').value = artist;
        editForm.querySelector('select[name="Mood"]').value = mood;
        editForm.querySelector('input[name="AudioUrl"]').value = audioUrl;
        editForm.querySelector('input[name="DurationInSeconds"]').value = duration;

        editForm.action = `/Tracks/Edit/${id}`;
        editBsModal.show();
    };

    editForm.addEventListener("submit", (e) => {
        e.preventDefault();

        const formData = new FormData(editForm);
        const trackId = formData.get("Id");

        fetch(editForm.action, {
            method: "POST",
            body: formData,
            headers: {
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(res => {
                if (!res.ok) throw new Error("Update failed");
                return res.json();
            })
            .then(data => {
                if (!data.success) throw new Error(data.message || "Update failed");

                editBsModal.hide();
                // Clean up modal backdrop and restore scroll
                cleanupModalBackdrop();

                // Update row content
                const row = document.querySelector(`button[onclick*="populateEditModal(${trackId}"]`)?.closest("tr");
                if (row) {
                    row.querySelector("td:nth-child(1)").textContent = data.title;
                    row.querySelector("td:nth-child(2)").textContent = data.artist;
                    row.querySelector("td:nth-child(3) .badge").textContent = data.mood;
                    row.querySelector("td:nth-child(4)").textContent = formatDuration(data.durationInSeconds);
                    row.querySelector("td:nth-child(5) audio source").src = data.audioUrl;
                    row.querySelector("td:nth-child(5) audio").load();

                    const editBtn = row.querySelector("button[onclick*='populateEditModal']");
                    if (editBtn) {
                        editBtn.setAttribute("onclick", `populateEditModal(${trackId}, '${data.title}', '${data.artist}', '${data.mood}', '${data.audioUrl}', ${data.durationInSeconds})`);
                    }

                    const deleteBtn = row.querySelector("button[onclick*='prepareDeleteModal']");
                    if (deleteBtn) {
                        deleteBtn.setAttribute("onclick", `prepareDeleteModal(${trackId}, '${data.title}')`);
                    }
                }

                toastBody.textContent = "Track updated successfully!";
                successToast.show();
            })
            .catch(err => alert(err.message));
    });

    // Format seconds to m:ss
    function formatDuration(seconds) {
        const minutes = Math.floor(seconds / 60);
        const secs = seconds % 60;
        return `${minutes}:${secs.toString().padStart(2, '0')}`;
    }
});
