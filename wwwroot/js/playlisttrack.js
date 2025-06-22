document.addEventListener("DOMContentLoaded", function () {
    const playlistSelect = document.getElementById("playlistId");
    const selectedPlaylistId = playlistSelect.value;

    if (selectedPlaylistId) {
        document.getElementById("trackId").disabled = false;
        loadTracks();
        loadPlaylistTracks(selectedPlaylistId);
    }

    playlistSelect.addEventListener("change", function () {
        const playlistId = this.value;
        if (playlistId) {
            document.getElementById("trackId").disabled = false;
            loadTracks();
            loadPlaylistTracks(playlistId);
        } else {
            document.getElementById("trackId").disabled = true;
            document.getElementById("tracksTableBody").innerHTML = "";
            document.getElementById("emptyState").style.display = "none";
        }
    });

    document.getElementById("addTrackForm").addEventListener("submit", function (e) {
        e.preventDefault();
        addTrackToPlaylist();
    });
}

);

function loadTracks() {
    fetch('/Tracks/GetAll')
        .then(res => res.json())
        .then(data => {
            const select = document.getElementById('trackId');
            select.innerHTML = '<option value="">Select a track</option>';
            data.forEach(t => {
                const option = document.createElement('option');
                option.value = t.id;
                option.textContent = `${t.title} - ${t.artist}`;
                select.appendChild(option);
            });
        })
        .catch(err => {
            console.error('Failed to load tracks:', err);
            showToast("Could not load tracks");
        });
}

function loadPlaylistTracks(playlistId) {
    fetch(`/Playlists/GetDetails/${playlistId}`)
        .then(res => res.json())
        .then(data => {
            const tbody = document.getElementById("tracksTableBody");
            tbody.innerHTML = "";

            if (data.tracks.length === 0) {
                document.getElementById("emptyState").style.display = "block";
                return;
            }

            document.getElementById("emptyState").style.display = "none";

            data.tracks.forEach(track => {
                const tr = document.createElement("tr");
                tr.innerHTML = `
                    <td>${track.position}</td>
                    <td>${data.title}</td>
                    <td>${track.title}</td>
                    <td>${track.artist}</td>
                    <td>
                        <button class="btn btn-sm btn-danger" onclick="removeTrack(${data.id}, ${track.id})">Remove</button>
                    </td>
                `;
                tbody.appendChild(tr);
            });
        })
        .catch(err => {
            console.error("Failed to load playlist tracks:", err);
            showToast("Could not load playlist tracks");
        });
}

function addTrackToPlaylist() {
    const playlistId = document.getElementById("playlistId").value;
    const trackId = document.getElementById("trackId").value;
    const position = document.getElementById("position").value;

    if (!playlistId || !trackId || !position) {
        showToast("Please fill out all fields.");
        return;
    }

    const dto = {
        playlistId: parseInt(playlistId),
        trackId: parseInt(trackId),
        position: parseInt(position)
    };

    fetch('/PlaylistTracks/Add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        },
        body: JSON.stringify(dto)
    })
        .then(response => {
            if (!response.ok) return response.text().then(text => { throw new Error(text); });
            return response.json();
        })
        .then(result => {
            showToast(result.message || "Track added successfully");
            loadPlaylistTracks(dto.playlistId);
            document.getElementById("addTrackForm").reset();
            document.getElementById("trackId").disabled = true;
        })
        .catch(err => {
            console.error("Failed to add track:", err);
            showToast("Error: " + err.message);
        });
}

function removeTrack(playlistId, trackId) {
    const dto = {
        playlistId: playlistId,
        trackId: trackId,
        position: 0
    };

    fetch('/PlaylistTracks/Remove', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        },
        body: JSON.stringify(dto)
    })
        .then(response => {
            if (!response.ok) return response.text().then(text => { throw new Error(text); });
            return response.text();
        })
        .then(() => {
            showToast("Track removed successfully");
            loadPlaylistTracks(playlistId);
        })
        .catch(err => {
            console.error("Failed to remove track:", err);
            showToast("Error: " + err.message);
        });
}
