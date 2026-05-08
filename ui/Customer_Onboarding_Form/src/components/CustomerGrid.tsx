import DataGrid, {
  Column,
  Paging,
  Pager,
  SearchPanel,
  LoadPanel,
} from 'devextreme-react/data-grid'

import type { Customer } from '../types/customer'

type Props = {
  data: Customer[]
  loading?: boolean
  onView: (id: string) => void
}

export default function CustomerGrid({
  data,
  loading,
  onView,
}: Props) {
  return (
    <DataGrid
      dataSource={data}
      keyExpr="id"
      showBorders={true}
    >
      <LoadPanel enabled={loading ?? false} />

      <SearchPanel visible={true} />

      <Paging defaultPageSize={10} />
      <Pager
        showPageSizeSelector={true}
        allowedPageSizes={[10, 20, 50]}
      />

      <Column dataField="id" caption="Customer ID" />
      <Column dataField="firstName" />
      <Column dataField="lastName" />
      <Column dataField="email" />
      <Column dataField="phoneNumber" />
      <Column dataField="createdAt" />
      <Column dataField="updatedAt" />

      {/* 👁 ACTION COLUMN */}
      <Column
        caption="Actions"
        cellRender={(cell) => (
          <button onClick={() => onView(cell.data.id)}>
            View
          </button>
        )}
      />
    </DataGrid>
  )
}