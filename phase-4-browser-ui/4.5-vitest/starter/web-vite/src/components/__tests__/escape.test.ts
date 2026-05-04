import { describe, it, expect } from 'vitest';
import { escapeHtml } from '../escape';

describe('escapeHtml', () => {
  it('escapes the five characters', () => {
    expect(escapeHtml('<script>')).toBe('&lt;script&gt;');
    expect(escapeHtml('"')).toBe('&quot;');
    expect(escapeHtml("'")).toBe('&#039;');
    expect(escapeHtml('A & B')).toBe('A &amp; B');
  });

  it('leaves safe strings alone', () => {
    expect(escapeHtml('hello world')).toBe('hello world');
  });
});
