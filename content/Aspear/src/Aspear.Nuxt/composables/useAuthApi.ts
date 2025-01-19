import { useApi } from "#imports";

function useAuthApi(
  ...args: Parameters<typeof useApi>
): ReturnType<typeof useApi> {
  const { $toast } = useNuxtApp();

  const path = toValue(args[0]);
  const opt = args[1] ?? {
    headers: [],
  };
  opt.key = `${path} ${JSON.stringify(opt)}`;
  opt.onRequestError = (error) => {
    console.log(error);
    $toast.error("Something wrong happened...", { timeout: 5000 });
  };

  return useApi(path, opt);
}

export default useAuthApi as typeof useApi;
