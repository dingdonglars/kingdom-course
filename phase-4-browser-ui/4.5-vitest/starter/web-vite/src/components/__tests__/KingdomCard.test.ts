import { describe, it, expect } from 'vitest';
import { KingdomCard } from '../KingdomCard';

describe('KingdomCard', () => {
  it('renders the kingdom name and day', () => {
    const html = KingdomCard({ id: 1, name: 'Eldoria', day: 11 });
    expect(html).toContain('Eldoria');
    expect(html).toContain('Day 11');
  });

  it('escapes the name', () => {
    const html = KingdomCard({ id: 1, name: '<script>x</script>', day: 1 });
    expect(html).toContain('&lt;script&gt;');
    expect(html).not.toContain('<script>');
  });
});
