

-- =============== STOCK (existencias actuales por producto) ===============
CREATE TABLE IF NOT EXISTS stock (
  prod_id        VARCHAR(13)  PRIMARY KEY REFERENCES producto(prod_id),
  stock_cantidad NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (stock_cantidad >= 0)
);

-- =============== AJUSTES DE INVENTARIO (cabecera/detalle) ===============
CREATE TABLE IF NOT EXISTS ajustes (
  ajus_id     SERIAL PRIMARY KEY,
  ajustp_id   INTEGER NOT NULL REFERENCES ajustes_tipo(ajustp_id),
  usua_id     INTEGER REFERENCES usuario(usua_id),
  ajus_fecha  DATE NOT NULL DEFAULT CURRENT_DATE,
  ajus_hora   TIME NOT NULL DEFAULT CURRENT_TIME,
  ajus_estado VARCHAR(12) DEFAULT 'PENDIENTE',
  ajus_obs    VARCHAR(120)
);


CREATE TABLE IF NOT EXISTS ajustes_detalle (
  ajusdet_id  SERIAL PRIMARY KEY,
  ajus_id     INTEGER NOT NULL REFERENCES ajustes(ajus_id) ON DELETE CASCADE,
  prod_id     VARCHAR(13) NOT NULL REFERENCES producto(prod_id),
  cantidad    NUMERIC(12,2) NOT NULL,             -- puede ser positiva o negativa
  observacion VARCHAR(120)
);


-- =============== PRESUPUESTO DETALLE ===============
CREATE TABLE IF NOT EXISTS presupuesto_detalle (
  presdet_id       SERIAL PRIMARY KEY,
  pres_id          INTEGER NOT NULL REFERENCES presupuesto(pres_id) ON DELETE CASCADE,
  prod_id          VARCHAR(13) NOT NULL REFERENCES producto(prod_id),
  presdet_cantidad NUMERIC(12,2) NOT NULL CHECK (presdet_cantidad > 0),
  presdet_precio   NUMERIC(12,2) NOT NULL CHECK (presdet_precio >= 0),
  presdet_descuento NUMERIC(12,2) DEFAULT 0 CHECK (presdet_descuento >= 0)
);


-- =============== PEDIDO DE CLIENTE DETALLE ===============
CREATE TABLE IF NOT EXISTS pedido_cliente_detalle (
  pedcldet_id     SERIAL PRIMARY KEY,
  pedcl_id        INTEGER NOT NULL REFERENCES pedido_cliente(pedcl_id) ON DELETE CASCADE,
  prod_id         VARCHAR(13) NOT NULL REFERENCES producto(prod_id),
  pedcldet_cantidad NUMERIC(12,2) NOT NULL CHECK (pedcldet_cantidad > 0),
  pedcldet_precio NUMERIC(12,2) NOT NULL CHECK (pedcldet_precio >= 0)
);

-- (opcional) relación PedidoCliente <-> Venta
CREATE TABLE IF NOT EXISTS pedido_cliente_has_venta (
  pedcl_id INTEGER NOT NULL REFERENCES pedido_cliente(pedcl_id) ON DELETE CASCADE,
  ven_id   INTEGER NOT NULL REFERENCES venta(ven_id) ON DELETE CASCADE,
  PRIMARY KEY (pedcl_id, ven_id)
);

-- =============== PEDIDO A PROVEEDOR DETALLE ===============
CREATE TABLE IF NOT EXISTS pedido_a_proveedor_detalle (
  pprovd_id        SERIAL PRIMARY KEY,
  pprov_id         INTEGER NOT NULL REFERENCES pedido_a_proveedor(pprov_id) ON DELETE CASCADE,
  prod_id          VARCHAR(13) NOT NULL REFERENCES producto(prod_id),
  pprovd_cantidad  NUMERIC(12,2) NOT NULL CHECK (pprovd_cantidad > 0),
  pprovd_precio    NUMERIC(12,2) NOT NULL CHECK (pprovd_precio >= 0)
);

-- =============== ORDEN DE COMPRA DETALLE ===============
CREATE TABLE IF NOT EXISTS orden_de_compra_detalle (
  ordet_id       SERIAL PRIMARY KEY,
  ordcom_id      INTEGER NOT NULL REFERENCES orden_de_compra(ordcom_id) ON DELETE CASCADE,
  prod_id        VARCHAR(13) NOT NULL REFERENCES producto(prod_id),
  ordet_cantidad NUMERIC(12,2) NOT NULL CHECK (ordet_cantidad > 0),
  ordet_precio   NUMERIC(12,2) NOT NULL CHECK (ordet_precio >= 0)
);
-- =============== COMPRA DETALLE ===============
CREATE TABLE IF NOT EXISTS compra_detalle (
  compdet_id       SERIAL PRIMARY KEY,
  comp_id          INTEGER NOT NULL REFERENCES compra(comp_id) ON DELETE CASCADE,
  prod_id          VARCHAR(13) NOT NULL REFERENCES producto(prod_id),
  compdet_cantidad NUMERIC(12,2) NOT NULL CHECK (compdet_cantidad > 0),
  compdet_precio   NUMERIC(12,2) NOT NULL CHECK (compdet_precio >= 0)
);


-- =============== ORDEN DE PAGO DETALLE (si no la tenías) ===============
CREATE TABLE IF NOT EXISTS orden_de_pago_detalle (
  ordpagdet_id   SERIAL PRIMARY KEY,
  ordpag_id      INTEGER NOT NULL REFERENCES orden_de_pago(ordpag_id) ON DELETE CASCADE,
  comp_id        INTEGER REFERENCES compra(comp_id),
  ordpag_numdoc  VARCHAR(20),
  ordpag_monto   NUMERIC(12,2) NOT NULL CHECK (ordpag_monto >= 0)
);

-- =============== DEVOLUCIÓN A PROVEEDOR DETALLE ===============
CREATE TABLE IF NOT EXISTS devolucion_a_proveedor_detalle (
  devprovdet_id       SERIAL PRIMARY KEY,
  devprov_id          INTEGER NOT NULL REFERENCES devolucion_a_proveedor(devprov_id) ON DELETE CASCADE,
  prod_id             VARCHAR(13) NOT NULL REFERENCES producto(prod_id),
  devprovdet_cantidad NUMERIC(12,2) NOT NULL CHECK (devprovdet_cantidad > 0),
  devprovdet_precio   NUMERIC(12,2) NOT NULL CHECK (devprovdet_precio >= 0)
);

-- =============== VENTA DETALLE ===============
CREATE TABLE IF NOT EXISTS venta_detalle (
  vendet_id       SERIAL PRIMARY KEY,
  ven_id          INTEGER NOT NULL REFERENCES venta(ven_id) ON DELETE CASCADE,
  prod_id         VARCHAR(13) NOT NULL REFERENCES producto(prod_id),
  vendet_cantidad NUMERIC(12,2) NOT NULL CHECK (vendet_cantidad > 0),
  vendet_precio   NUMERIC(12,2) NOT NULL CHECK (vendet_precio >= 0),
  vendet_descuento NUMERIC(12,2) DEFAULT 0 CHECK (vendet_descuento >= 0)
);

-- =============== CUENTA A COBRAR (ventas a crédito) ===============
CREATE TABLE IF NOT EXISTS cuenta_a_cobrar (
  cacob_id        SERIAL PRIMARY KEY,
  ven_id          INTEGER NOT NULL REFERENCES venta(ven_id) ON DELETE CASCADE,
  cuenta_documento VARCHAR(30),
  cuenta_monto     NUMERIC(12,2) NOT NULL CHECK (cuenta_monto >= 0),
  cuenta_vencimiento DATE,
  cuenta_estado     VARCHAR(12) DEFAULT 'PENDIENTE'
);

-- =============== COBRO DETALLE (aplicaciones del cobro) ===============
CREATE TABLE IF NOT EXISTS cobro_detalle (
  cobdet_id  SERIAL PRIMARY KEY,
  cob_id     INTEGER NOT NULL REFERENCES cobro(cob_id) ON DELETE CASCADE,
  cacob_id   INTEGER NOT NULL REFERENCES cuenta_a_cobrar(cacob_id) ON DELETE CASCADE,
  monto      NUMERIC(12,2) NOT NULL CHECK (monto >= 0)
);