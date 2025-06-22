document.addEventListener('DOMContentLoaded', () => {



    const searchInput = document.getElementById("searchInput");
    const playlistItems = document.querySelectorAll(".playlist-item");
    const totalCount = document.getElementById("totalCount");

    function filterPlaylists() {
        const query = searchInput.value.trim().toLowerCase();
        let visibleCount = 0;

        playlistItems.forEach(item => {
            const title = item.getAttribute("data-name")?.toLowerCase() || "";
            if (title.includes(query)) {
                item.style.display = "";
                visibleCount++;
            } else {
                item.style.display = "none";
            }
        });

        if (totalCount) {
            totalCount.textContent = visibleCount;
        }
    }

    // Filter while typing
    if (searchInput) {
        searchInput.addEventListener("input", filterPlaylists);
    }

    //  filter on pressing Enter or clicking the search button
    const searchBtn = document.querySelector(".input-group button");
    if (searchBtn) {
        searchBtn.addEventListener("click", filterPlaylists);
    }

   
    // Toast setup
    const toastEl = document.getElementById('successToast');
    const toastBody = toastEl.querySelector('.toast-body');
    const successToast = new bootstrap.Toast(toastEl);

    const teamFilter = document.getElementById('teamFilter');

    if (teamFilter) {
        teamFilter.addEventListener('change', () => {
            const selectedTeamId = teamFilter.value;

            playlistItems.forEach(item => {
                const itemTeamId = item.getAttribute('data-team-id');
                item.style.display = !selectedTeamId || itemTeamId === selectedTeamId ? '' : 'none';
            });

            const totalCount = document.getElementById('totalCount');
            if (totalCount) {
                const visibleCount = Array.from(playlistItems).filter(item => item.style.display !== 'none').length;
                totalCount.textContent = visibleCount;
            }
        });
    }


    // DELETE MODAL SETUP
    const deleteModal = document.getElementById("deletePlaylistModal");
    const deleteForm = deleteModal.querySelector("form");
    const deletePlaylistIdInput = deleteForm.querySelector("input[name='id']");
    const deleteModalBody = deleteModal.querySelector(".modal-body");
    const deleteBsModal = new bootstrap.Modal(deleteModal);

    document.querySelectorAll(".delete-btn").forEach(button => {
        button.addEventListener("click", () => {
            const playlistId = button.getAttribute("data-playlist-id");
            const playlistTitle = button.getAttribute("data-playlist-title");

            deletePlaylistIdInput.value = playlistId;
            deleteModalBody.textContent = `Are you sure you want to delete "${playlistTitle}"?`;
            deleteForm.action = `/Playlists/Delete/${playlistId}`;

            deleteBsModal.show();
        });
    });

    deleteForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const formData = new FormData(deleteForm);

        fetch(deleteForm.action, {
            method: "POST",
            body: formData,
            headers: {
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(response => {
                if (!response.ok) throw new Error("Failed to delete playlist.");
                return response.json();
            })
            .then(data => {
                if (!data.success) throw new Error(data.message || "Delete failed");

                deleteBsModal.hide();
                cleanupModalBackdrop();

                const card = document.querySelector(`button.delete-btn[data-playlist-id="${formData.get('id')}"]`).closest('.playlist-item');
                if (card) card.remove();

                toastBody.textContent = 'Playlist deleted successfully!';
                successToast.show();
            })
            .catch(err => alert(err.message));
    });

    // EDIT MODAL SETUP
    const editModal = document.getElementById("editPlaylistModal");
    const editForm = editModal.querySelector("form");
    const editBsModal = new bootstrap.Modal(editModal);

    document.querySelectorAll(".edit-btn").forEach(button => {
        button.addEventListener("click", () => {
            const playlistId = button.getAttribute("data-playlist-id");
            const title = button.getAttribute("data-playlist-title");
            const teamId = button.getAttribute("data-playlist-teamid");
            const description = button.getAttribute("data-playlist-description");
            const coverImageUrl = button.getAttribute("data-playlist-coverimageurl");

            editForm.querySelector('input[name="Id"]').value = playlistId;
            editForm.querySelector('input[name="Title"]').value = title;
            editForm.querySelector('select[name="TeamId"]').value = teamId;
            editForm.querySelector('textarea[name="Description"]').value = description;

            // IMPORTANT: Use the correct hidden input name for existing image url
            editForm.querySelector('input[name="ExistingCoverImageUrl"]').value = coverImageUrl || '';

            editBsModal.show();
        });
    });

    editForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const formData = new FormData(editForm);
        const playlistId = formData.get("Id");

        fetch(editForm.action, {
            method: "POST",
            body: formData,
            headers: {
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(response => {
                if (!response.ok) throw new Error("Failed to update playlist.");
                return response.json();
            })
            .then(data => {
                editBsModal.hide();
                cleanupModalBackdrop();

                const card = document.querySelector(`button.edit-btn[data-playlist-id="${playlistId}"]`).closest('.playlist-item');
                if (card) {
                    card.querySelector('.card-title').textContent = data.title;

                    const badge = card.querySelector('.team-badge.bg-primary');
                    if (badge) badge.textContent = data.teamName;

                    const descEl = card.querySelector('.playlist-description');
                    if (descEl) descEl.textContent = data.description;

                    card.setAttribute('data-team-id', data.teamId);
                    card.setAttribute('data-name', data.title);

                    // Update cover image in UI if exists
                    const imgEl = card.querySelector('img.playlist-cover');
                    if (imgEl && data.coverImageUrl) {
                        imgEl.src = data.coverImageUrl;
                    }
                }

                const editBtn = document.querySelector(`button.edit-btn[data-playlist-id="${playlistId}"]`);
                if (editBtn) {
                    editBtn.setAttribute("data-playlist-title", data.title);
                    editBtn.setAttribute("data-playlist-teamid", data.teamId);
                    editBtn.setAttribute("data-playlist-description", data.description);
                    editBtn.setAttribute("data-playlist-coverimageurl", data.coverImageUrl || '');
                }

                const deleteBtn = document.querySelector(`button.delete-btn[data-playlist-id="${playlistId}"]`);
                if (deleteBtn) {
                    deleteBtn.setAttribute("data-playlist-title", data.title);
                }

                toastBody.textContent = 'Playlist updated successfully!';
                successToast.show();

                const updatedPlaylistItems = document.querySelectorAll('.playlist-item');
                const selectedTeamId = teamFilter.value;

                updatedPlaylistItems.forEach(item => {
                    const itemTeamId = item.getAttribute('data-team-id');
                    item.style.display = !selectedTeamId || itemTeamId === selectedTeamId ? '' : 'none';
                });

                const totalCount = document.getElementById('totalCount');
                if (totalCount) {
                    const visibleCount = Array.from(updatedPlaylistItems).filter(item => item.style.display !== 'none').length;
                    totalCount.textContent = visibleCount;
                }
            })
            .catch(err => alert(err.message));
    });
});
