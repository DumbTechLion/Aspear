// index.d.ts
declare module "#app" {
  interface SubElement {
    icon?: string;
    title: string;
  }
  interface ActionButton {
    icon?: string;
    title: string;
    color?: "primary" | "secondary" | "success" | "warning" | "danger";
    variant?: "text" | "filled" | "outlined";
    href?: string;
    onClick?: () => void;
    alwaysVisible?: boolean;
  }
  interface BreadcrumbItem {
    title: string;
    href?: string;
    onClick?: () => void;
  }

  interface LayoutMetadata {
    title: string;
    titleIcon?: string;
    breadcrumbs?: BreadcrumbItem[];
    actions?: ActionButton[];
    subElements?: SubElement[];
  }

  interface PageMeta {
    layoutMetadata?: LayoutMetadata;
  }
}

export {};
