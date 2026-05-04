const API = 'https://localhost:5xxx';   // CHANGE to your API port

async function loadKingdom() {
  try {
    const resp = await fetch(`${API}/kingdoms`);
    if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
    const slots = await resp.json();
    if (slots.length === 0) {
      document.querySelector('main').textContent = "No kingdoms yet — create one via the API.";
      return;
    }
    renderSummary(slots[0]);
  } catch (err) {
    console.error('Failed to load kingdom:', err);
    document.querySelector('main').textContent = `Error: ${err.message}`;
  }
}

function renderSummary(slot) {
  document.getElementById('day').textContent = slot.day;
  document.querySelector('h1').textContent = slot.name;
}

loadKingdom();
