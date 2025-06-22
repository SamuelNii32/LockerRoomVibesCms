document.addEventListener("DOMContentLoaded", function () {
    // SEARCH FUNCTIONALITY
    const searchInput = document.getElementById("searchInput");
    const searchButton = document.querySelector(".search-box button");
    const rows = document.querySelectorAll("#teamsTableBody tr");
    const totalCount = document.getElementById("totalCount");

    if (searchInput && searchButton && rows.length) {
        function filterTeams() {
            const query = searchInput.value.trim().toLowerCase();
            let visibleCount = 0;

            rows.forEach(row => {
                const name = row.getAttribute("data-team-name")?.toLowerCase() || "";
                if (name.includes(query)) {
                    row.style.display = "";
                    visibleCount++;
                } else {
                    row.style.display = "none";
                }
            });

            if (totalCount) {
                totalCount.textContent = visibleCount;
            }
        }

        searchInput.addEventListener("input", filterTeams);
        searchButton.addEventListener("click", filterTeams);
    }

    // DELETE MODAL SETUP
    const deleteModal = document.getElementById("deleteModal");
    const teamIdInput = document.getElementById("teamIdToDelete");
    const teamNameSpan = document.getElementById("teamNameToDelete");
    const deleteForm = document.getElementById("deleteForm");
    const deleteBsModal = new bootstrap.Modal(deleteModal);

    document.querySelectorAll(".delete-btn").forEach(button => {
        button.addEventListener("click", () => {
            const teamId = button.getAttribute("data-team-id");
            const teamName = button.getAttribute("data-team-name");

            teamIdInput.value = teamId;
            teamNameSpan.textContent = teamName;

            deleteBsModal.show();
        });
    });

    deleteForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const formData = new FormData(deleteForm);
        const teamId = formData.get("id");

        fetch(deleteForm.action, {
            method: "POST",
            body: formData,
            headers: {
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(response => {
                if (!response.ok) throw new Error("Failed to delete team.");
                return response.json();
            })
            .then(data => {
                if (!data.success) throw new Error(data.message || "Delete failed");

                deleteBsModal.hide();
                cleanupModalBackdrop();

                const row = document.querySelector(`button.delete-btn[data-team-id="${teamId}"]`)?.closest("tr");
                if (row) row.remove();

                showToast("Team deleted successfully!");
            })
            .catch(error => {
                showToast("Error: " + error.message);
            });
    });

    // EDIT MODAL SETUP
    const editModal = document.getElementById("editModal");
    const editForm = document.getElementById("editForm");

    document.querySelectorAll(".edit-btn").forEach(button => {
        button.addEventListener("click", function () {
            const teamId = this.getAttribute("data-team-id");
            const teamName = this.getAttribute("data-team-name");
            const logoUrl = this.getAttribute("data-team-logourl");

            document.getElementById("editTeamId").value = teamId;
            document.getElementById("editTeamName").value = teamName;

            const previewImg = document.getElementById("editLogoPreview");
            previewImg.src = logoUrl || "https://via.placeholder.com/80";
            previewImg.alt = `${teamName} Logo`;
            previewImg.style.display = "block";

            const editBsModal = new bootstrap.Modal(editModal);
            editBsModal.show();
        });
    });

    editForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const formData = new FormData(editForm);
        const teamId = formData.get("id");

        fetch(editForm.action, {
            method: "POST",
            body: formData,
            headers: {
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(response => {
                if (!response.ok) throw new Error("Failed to update team.");
                return response.json();
            })
            .then(data => {
                const bsModal = bootstrap.Modal.getInstance(editModal);
                bsModal.hide();
                cleanupModalBackdrop();

                const row = document.querySelector(`button.edit-btn[data-team-id="${teamId}"]`)?.closest("tr");
                if (row) {
                    row.querySelector("td:nth-child(2) strong").textContent = data.name;

                    const img = row.querySelector("td:first-child img");
                    img.src = data.logoUrl || "https://via.placeholder.com/40";
                    img.alt = data.name + " Logo";

                    const editBtn = row.querySelector("button.edit-btn");
                    editBtn.setAttribute("data-team-name", data.name);
                    editBtn.setAttribute("data-team-logourl", data.logoUrl || "");

                    const deleteBtn = row.querySelector("button.delete-btn");
                    deleteBtn.setAttribute("data-team-name", data.name);
                }

                showToast("Team updated successfully!");
            })
            .catch(error => {
                showToast("Error: " + error.message);
            });
    });
});
