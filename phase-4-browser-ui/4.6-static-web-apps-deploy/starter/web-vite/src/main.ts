import './style.css';
import type { KingdomSlot } from './types';
import { KingdomCard } from './components/KingdomCard';

const API = import.meta.env.VITE_API_URL;

async function main() {
  const root = document.querySelector<HTMLDivElement>('#app')!;
  try {
    const resp = await fetch(`${API}/kingdoms`, { credentials: 'include' });
    if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
    const slots = (await resp.json()) as KingdomSlot[];

    if (slots.length === 0) {
      root.innerHTML = '<p>No kingdoms yet.</p>';
      return;
    }
    root.innerHTML = slots.map(KingdomCard).join('');
  } catch (err) {
    root.textContent = `Error: ${(err as Error).message}`;
  }
}

main();
