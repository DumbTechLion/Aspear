export const useDashboard = () =>
  useCookie<DashboardState>("dashboard", { default: () => ({}) });

export interface DashboardState {
  organizationId?: string;
  organizationName?: string;
}
