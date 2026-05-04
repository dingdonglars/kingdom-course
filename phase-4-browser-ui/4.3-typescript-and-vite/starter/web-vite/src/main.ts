import './style.css';
import type { KingdomSlot } from './types';

const API = 'https://localhost:5xxx';   // CHANGE to your API port

async function loadKingdom(): Promise<KingdomSlot | null> {
  const resp = await fetch(`${API}/kingdoms`);
  if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
  const slots = (await resp.json()) as KingdomSlot[];
  return slots[0] ?? null;
}

function render(slot: KingdomSlot | null) {
  const root = document.querySelector<HTMLDivElement>('#app')!;
  if (!slot) {
    root.innerHTML = '<p>No kingdoms yet.</p>';
    return;
  }
  root.innerHTML = `
    <header>
      <h1>${slot.name}</h1>
      <p>Day ${slot.day}</p>
    </header>
  `;
}

loadKingdom().then(render).catch(err => {
  document.querySelector<HTMLDivElement>('#app')!.textContent = `Error: ${err.message}`;
});
