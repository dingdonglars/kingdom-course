import type { KingdomSlot } from '../types';
import { escapeHtml } from './escape';

export function KingdomCard(slot: KingdomSlot): string {
  return `
    <article class="card">
      <h2>${escapeHtml(slot.name)}</h2>
      <p>Day ${slot.day}</p>
    </article>
  `;
}
