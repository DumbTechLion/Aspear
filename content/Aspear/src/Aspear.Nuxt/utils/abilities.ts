import type { User } from "#auth-utils";
import { OrganizationRole, type components } from "~/.nuxt/types/open-fetch/schemas/api";
type CustomerOrganizationUser = components["schemas"]["AlfaBull.Business.Features.Customers.Organizations.Contracts.OrganizationUserDto"];

export const listOrganizations = defineAbility(() => true); // Only authenticated users can list posts

export const manageUsersInOrganization = defineAbility(
  (user: User, organizationUser: CustomerOrganizationUser) => {
    return user.uuid == organizationUser.userId 
      && organizationUser.role
      && [OrganizationRole.Admin, OrganizationRole.Editor].includes(organizationUser.role);
  }
);
